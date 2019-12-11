namespace VoodooPackages.Tech.Items
{
    public class PaymentButton : PaymentPicker
    {
        public PaymentCurrency payment;

        private void Start()
        {
            Display();
        }

        private void Display()
        {
            if (payment != null) 
                Display(payment);
        }

        public void Purchase()
        {
            payment.Purchase();
            Display();
        }
    }
}