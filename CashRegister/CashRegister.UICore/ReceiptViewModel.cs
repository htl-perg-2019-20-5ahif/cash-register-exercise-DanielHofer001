using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Text;

namespace CashRegister.UICore
{
    public class ReceiptViewModel : BindableBase
    {
        
            private int id;
            public int Id
            {
                get { return id; }
                set { SetProperty(ref id, value); }
            }
            private int amount;

            public int Amount
            {
                get { return amount; }
                set { SetProperty(ref amount, value); }
            }
            private decimal totalPrice;

            public decimal TotalPrice
            {
                get { return totalPrice; }
                set { SetProperty(ref totalPrice, value); }
            }
            private string name;

            public string Name
            {
                get { return name; }
                set { SetProperty(ref name, value); }
            }
        }
    }
