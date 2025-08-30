using Common;
using Modbus.FunctionParameters;
using System;
using System.Collections.Generic;
using System.Net;
using System.Reflection;

namespace Modbus.ModbusFunctions
{
    /// <summary>
    /// Class containing logic for parsing and packing modbus write coil functions/requests.
    /// </summary>
    public class WriteSingleCoilFunction : ModbusFunction
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="WriteSingleCoilFunction"/> class.
        /// </summary>
        /// <param name="commandParameters">The modbus command parameters.</param>
        public WriteSingleCoilFunction(ModbusCommandParameters commandParameters) : base(commandParameters)
        {
            CheckArguments(MethodBase.GetCurrentMethod(), typeof(ModbusWriteCommandParameters));
        }

        /// <inheritdoc />
        public override byte[] PackRequest()
        {
            //TO DO: IMPLEMENT
            ModbusWriteCommandParameters readParams = (ModbusWriteCommandParameters)CommandParameters;

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
            Buffer.BlockCopy(BitConverter.GetBytes(IPAddress.HostToNetworkOrder((short)readParams.OutputAddress)), 0, request, offset, 2);
            offset += 2;

            // Quantity (2 bajta)
            Buffer.BlockCopy(BitConverter.GetBytes(IPAddress.HostToNetworkOrder((short)readParams.Value)), 0, request, offset, 2);
            offset += 2;

            return request;
        }

        /// <inheritdoc />
        public override Dictionary<Tuple<PointType, ushort>, ushort> ParseResponse(byte[] response)
        {
            //TO DO: IMPLEMENT
            Dictionary<Tuple<PointType, ushort>, ushort> rijecnik = new Dictionary<Tuple<PointType, ushort>, ushort>();

            ModbusWriteCommandParameters writeParams = (ModbusWriteCommandParameters)CommandParameters;

            // Adresa na koju je upisano (bajtovi 8 i 9)
            ushort address = (ushort)((response[8] << 8) | response[9]);

            // Vrednost koja je upisana (bajtovi 10 i 11)
            ushort value = (ushort)((response[10] << 8) | response[11]);

            // Upisujemo u rečnik — po definiciji PointType-a, to je DIGITAL_OUTPUT
            rijecnik.Add(new Tuple<PointType, ushort>(PointType.DIGITAL_OUTPUT, address), value);

            return rijecnik;
        }
    }
}