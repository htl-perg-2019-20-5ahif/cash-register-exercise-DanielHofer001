using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CashRegister.Shared
{
    /*
     * Every product consists of:
     * ID (numeric, unique key)
     * Product name (mandatory)
     * Unit price (numeric, mandatory)
    */
    public class Product
    {

        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public decimal Price { get; set; }
    }
    /*
     *  Every receipt line consists of:
        ID (numeric, unique key)
        Reference to the bought product
        Amount of pieces bought
        Total price (numeric, amount * product's unit price, calculated by backend)
     */
    public class ReceiptLine
    {
        public int Id { get; set; }
        [Required]
        public int Amount { get; set; }
        [Required]
        public decimal TotalPrice { get; set; }
        [Required]
        public int ProductId { get; set; }
        public Product Product { get; set; }
        [Required]
        public int ReceiptId { get; set; }
        public Receipt Receipt { get; set; }
    }
    /*
     *  Every receipt consists of:
        A list of receipt lines (at least one)
        Receipt timestamp (auto-assigned by backend)
        Total price (numeric, sum of total prices of all receipt lines, calculated by backend)
     */
    public class Receipt
    {
        public int Id { get; set; }
        public List<ReceiptLine> ReceiptLines { get; set; }
        [Required]
        public DateTime TimeStamp { get; set; }
        [Required]
        public decimal TotalPrice { get; set; }
    }
}
