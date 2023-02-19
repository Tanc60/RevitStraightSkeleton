using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace AreaCalc.ModelDB.DataDefinition
{
    class ModelDBObject
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public long Id { get; set; }
        public string Guid { get; set; }
        public string UserLabel { get; set; }
        public string AlternateProperties { get; set; }
    }
}
