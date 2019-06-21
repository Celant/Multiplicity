using System.IO;

namespace Multiplicity.Packets
{
    /// <summary>
    /// The TravellingMerchantInventory (0x48) packet.
    /// </summary>
    public class TravellingMerchantInventory : TerrariaPacket
    {

        public short[] Items { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="TravellingMerchantInventory"/> class.
        /// </summary>
        public TravellingMerchantInventory()
            : base((byte)PacketTypes.TravellingMerchantInventory)
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TravellingMerchantInventory"/> class.
        /// </summary>
        /// <param name="br">br</param>
        public TravellingMerchantInventory(BinaryReader br)
            : base(br)
        {
            Items = new short[40];
            for (int i = 0; i < 40; i++)
                Items[i] = br.ReadInt16();
        }

        public override string ToString()
        {
            return string.Format("[TravellingMerchantInventory]");
        }

        #region implemented abstract members of TerrariaPacket

        public override short GetLength()
        {
            return (short)(80);
        }

        public override void ToStream(Stream stream, bool includeHeader = true)
        {
            /*
             * Length and ID headers get written in the base packet class.
             */
            if (includeHeader) {
                base.ToStream(stream, includeHeader);
            }

            /*
             * Always make sure to not close the stream when serializing.
             * 
             * It is up to the caller to decide if the underlying stream
             * gets closed.  If this is a network stream we do not want
             * the regressions of unconditionally closing the TCP socket
             * once the payload of data has been sent to the client.
             */
            using (BinaryWriter br = new BinaryWriter(stream, new System.Text.UTF8Encoding(), leaveOpen: true))
            {
                for (int i = 0; i < 40; i++)
                    br.Write(Items[i]);
            }
        }

        #endregion

    }
}
