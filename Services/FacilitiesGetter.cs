using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using UplLogEmail.Models.Data;
using UplLogEmail.Models.Tables;

namespace UplLogEmail.Services;

public class FacilitiesGetter : IFacilitiesGetter
{
    public async Task<List<FacilityData>> GetFacilities(DbContext context)
    {
        var facilities = await (
            from fac in context.Set<FacilityTable>()
            join pccFac in context.Set<PccFacilityTable>()
                on fac.SupcareFacId equals pccFac.SupcareFacId
            where pccFac.IsPccActive == 1 && pccFac.SupcareFacId > 0
            select new FacilityData
            {
                Id = fac.SupcareFacId,
                PccFacId = pccFac.PccFacId,
                OrgUuid = pccFac.PccOrgUuid,
                FacName = fac.FacName,
                Contacts = new List<FacilityContactData>(),
            }
        ).ToListAsync();

        var result = new List<FacilityData>();

        foreach (var facility in facilities)
        {
            var contacts = await context
                .Set<FacContactTable>()
                .Where(c => c.FacilityId == facility.Id && !string.IsNullOrEmpty(c.UplLogEmail))
                .Select(c => new FacilityContactData
                {
                    Id = c.FacContactId,
                    FacilityId = c.FacilityId,
                    UplLogEmail = c.UplLogEmail,
                })
                .ToListAsync();

            if (contacts.Count > 0)
            {
                facility.Contacts = contacts;
                result.Add(facility);
            }
        }

        return result;
    }
}

public interface IFacilitiesGetter
{
    Task<List<FacilityData>> GetFacilities(DbContext context);
}
