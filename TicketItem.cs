namespace ExampleCsPlugin
{
    public class TicketItem
    {
        private readonly string _ticketNumber;
        private readonly string _ticketSummary;
        private readonly string _ticketState;
        private readonly string _ticketPlanning;
        public TicketItem(string ticketNumber, string ticketState, string ticketSummary, string ticketPlanning)
        {
            _ticketNumber = ticketNumber;

            _ticketState = ticketState;
            _ticketSummary = ticketSummary;
            _ticketPlanning = ticketPlanning;
        }
        public string State
        {
            get { return _ticketState; }
        }
        public string Number
        {
            get { return _ticketNumber; }
        }

        public string Summary
        {
            get { return _ticketSummary; }
        }

        public string Planning
        {
            get { return _ticketPlanning; }
        }
    
    }

}