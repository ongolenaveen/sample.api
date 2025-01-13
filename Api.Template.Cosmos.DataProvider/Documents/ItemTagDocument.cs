using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Api.Template.Domain.Dto;

namespace Api.Template.Cosmos.DataProvider.Documents
{
    public class ItemTagDocument : ICosmosDocument
    {
        public string Id { get; set; } = string.Empty;

        public string PKey { get; set; } = string.Empty;

        public int? ItemId { get; set; }

        public Tag? Tags { get; set; }
    }
}
