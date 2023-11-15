using System;

namespace GSqlQuery.MySql.Test.Data.Table
{
    [Table("sakila", "inventory")]
    public class Inventory : EntityExecute<Inventory>
    {
        [Column("inventory_id", Size = 7, IsPrimaryKey = true, IsAutoIncrementing = true)]
        public int InventoryId { get; set; }

        [Column("film_id", Size = 5)]
        public long FilmId { get; set; }

        [Column("store_id", Size = 3)]
        public byte StoreId { get; set; }

        [Column("last_update", Size = 19)]
        public DateTime LastUpdate { get; set; }

        public Inventory()
        { }

        public Inventory(int inventoryId, long filmId, byte storeId, DateTime lastUpdate)
        {
            InventoryId = inventoryId;
            FilmId = filmId;
            StoreId = storeId;
            LastUpdate = lastUpdate;
        }
    }
}
