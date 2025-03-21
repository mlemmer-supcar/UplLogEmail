using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using UplLogEmail.Models.Data;
using UplLogEmail.Models.Tables;
using UplLogEmail.Utils;

namespace UplLogEmail.Services;

public class UploadLogGetter() : IUploadLogGetter
{
    public async Task<List<UploadLogData>> GetUploadedLogs(
        DbContext dbContext,
        FacilityData facility
    )
    {
        var yesterdayStart = DateTime.Today.AddDays(-1);
        var yesterdayEnd = DateTime.Today;

        var logs = await (
            from log in dbContext.Set<PccUploadLogTable>().AsNoTracking()
            join client in dbContext.Set<ClientInfoTable>().AsNoTracking()
                on log.SupCareId equals client.ClientID
            join provider in dbContext.Set<ProviderTable>().AsNoTracking()
                on log.ProviderId equals provider.ProviderId
            where
                log.FacId == facility.PccFacId
                && log.OrgUuid == facility.OrgUuid
                && log.UploadedDate >= yesterdayStart
                && log.UploadedDate < yesterdayEnd
            select new
            {
                UploadDate = log.UploadedDate,
                log.SessionDate,
                ClientName = client.LastName + ", " + client.FirstName,
                provider.ProviderName,
                log.FileType,
                log.NoteId,
            }
        ).ToListAsync();

        var result = logs.Select(l => new UploadLogData
            {
                UploadDate = l.UploadDate?.ToString("MM/dd/yyyy") ?? string.Empty,
                SessionDate = l.SessionDate?.ToString("MM/dd/yyyy") ?? string.Empty,
                ClientName = l.ClientName,
                ProviderName = l.ProviderName,
                NoteType = l.FileType,
                NoteId = l.NoteId,
            })
            .ToList();

        var phqNotes = result.Where(r => r.NoteType == "PHQ9").ToList();
        var bimsNotes = result.Where(r => r.NoteType == "BIMS").ToList();

        foreach (var note in phqNotes)
        {
            var phqNote = await dbContext
                .Set<PhqTable>()
                .AsNoTracking()
                .FirstOrDefaultAsync(p => p.PhqId == note.NoteId);

            if (phqNote != null)
            {
                note.Phq9Score = phqNote.Score?.ToString() ?? "";
                note.Phq2Score =
                    Phq2ScoreGetter.GetScore(
                        phqNote.Score ?? 0,
                        phqNote.InterviewConducted ?? "-1",
                        phqNote.Interest ?? "0",
                        phqNote.Down ?? "0"
                    ) ?? "";
            }
        }

        foreach (var note in bimsNotes)
        {
            var bimsNote = await dbContext
                .Set<BimsTable>()
                .AsNoTracking()
                .FirstOrDefaultAsync(b => "BXB" + b.BimsId == note.NoteId);

            if (bimsNote != null)
            {
                var log = result.First(r => r.NoteId == note.NoteId);
                log.BimsScore = bimsNote.BimsScore?.ToString() ?? "";
            }
        }

        return result;
    }
}

public interface IUploadLogGetter
{
    Task<List<UploadLogData>> GetUploadedLogs(DbContext dbContext, FacilityData facility);
}
