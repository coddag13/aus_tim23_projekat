using Common;
using Modbus.FunctionParameters;
using System;
using System.Collections.Generic;
using System.Net;
using System.Reflection;

namespace Modbus.ModbusFunctions
{
    /// <summary>
    /// Class containing logic for parsing and packing modbus read coil functions/requests.
    /// </summary>
    public class ReadCoilsFunction : ModbusFunction
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ReadCoilsFunction"/> class.
        /// </summary>
        /// <param name="commandParameters">The modbus command parameters.</param>
		public ReadCoilsFunction(ModbusCommandParameters commandParameters) : base(commandParameters)
        {
            CheckArguments(MethodBase.GetCurrentMethod(), typeof(ModbusReadCommandParameters));
        }

        /// <inheritdoc/>
        public override byte[] PackRequest()
        {
            //TO DO: IMPLEMENT
            ModbusReadCommandParameters readParams = (ModbusReadCommandParameters)CommandParameters;

            byte[] request = new byte[12];
            int offset = 0;

            // Transaction ID (2 bajta)
            Buffer.BlockCopy(BitConverter.GetBytes(IPAddress.HostToNetworkOrder((short)readParams.TransactionId)), 0, request, offset, 2);
            offset += 2;

            // Protocol ID (2 bajta)
            Buffer.BlockCopy(BitConverter.GetBytes(IPAddress.HostToNetworkOrder((short)readParams.ProtocolId)), 0, request, offset, 2);
            offset += 2;

            // Length (2 bajta)
            Buffer.BlockCopy(BitConverter.GetBytes(IPAddress.HostToNetworkOrder((short)readParams.Length)), 0, request, offset, 2);
            offset += 2;

            // Unit ID (1 bajt)
            request[offset++] = readParams.UnitId;

            // Function Code (1 bajt)
            request[offset++] = readParams.FunctionCode;

            // Start Address (2 bajta)
            Buffer.BlockCopy(BitConverter.GetBytes(IPAddress.HostToNetworkOrder((short)readParams.StartAddress)), 0, request, offset, 2);
            offset += 2;

            // Quantity (2 bajta)
            Buffer.BlockCopy(BitConverter.GetBytes(IPAddress.HostToNetworkOrder((short)readParams.Quantity)), 0, request, offset, 2);
            offset += 2;

            return request;
        }

        /// <inheritdoc />
        public override Dictionary<Tuple<PointType, ushort>, ushort> ParseResponse(byte[] response)
        {
            //TO DO: IMPLEMENT
            Dictionary<Tuple<PointType, ushort>, ushort> rijecnik = new Dictionary<Tuple<PointType, ushort>, ushort>();
            ModbusReadCommandParameters readParams = (ModbusReadCommandParameters)CommandParameters;


            int byteCount = response[8];

            for (int i = 0; i < byteCount; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    if ((j + i * 8) >= readParams.Quantity) break;
                    ushort v = (ushort)(response[9 + i] & (byte)0x1);
                    response[9 + i] /= 2;
                    rijecnik.Add(new Tuple<PointType, ushort>(PointType.DIGITAL_OUTPUT, (ushort)(readParams.StartAddress + (j + i * 8))), v);
                }
            }
            return rijecnik;
        }
    }
}