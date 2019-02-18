using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WS980
{
    class HistoricDataRecord
    {
        private DateTime recTime;
        private byte[] byteRecord;

        public HistoricDataRecord(DateTime recTime, byte[] byteRecord)
        {
            this.recTime = recTime;
            this.byteRecord = byteRecord;

            // Todo hier noch den historischen record auftröseln
        }
    }
}
