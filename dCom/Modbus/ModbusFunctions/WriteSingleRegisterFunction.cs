using Common;
using Modbus.FunctionParameters;
using System;
using System.Collections.Generic;
using System.Net;
using System.Reflection;

namespace Modbus.ModbusFunctions
{
    /// <summary>
    /// Class containing logic for parsing and packing modbus write single register functions/requests.
    /// </summary>
    public class WriteSingleRegisterFunction : ModbusFunction
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="WriteSingleRegisterFunction"/> class.
        /// </summary>
        /// <param name="commandParameters">The modbus command parameters.</param>
        public WriteSingleRegisterFunction(ModbusCommandParameters commandParameters) : base(commandParameters)
        {
            CheckArguments(MethodBase.GetCurrentMethod(), typeof(ModbusWriteCommandParameters));
        }

        /// <inheritdoc />
        public override byte[] PackRequest()
        {
            //TO DO: IMPLEMENT
            ModbusWriteCommandParameters writeParams = (ModbusWriteCommandParameters)CommandParameters;

            byte[] request = new byte[12];
            int offset = 0;

            // Transaction ID
            Buffer.BlockCopy(BitConverter.GetBytes(IPAddress.HostToNetworkOrder((short)writeParams.TransactionId)), 0, request, offset, 2);
            offset += 2;

            // Protocol ID
            Buffer.BlockCopy(BitConverter.GetBytes(IPAddress.HostToNetworkOrder((short)writeParams.ProtocolId)), 0, request, offset, 2);
            offset += 2;

            // Length
            Buffer.BlockCopy(BitConverter.GetBytes(IPAddress.HostToNetworkOrder((short)writeParams.Length)), 0, request, offset, 2);
            offset += 2;

            // Unit ID
            request[offset++] = writeParams.UnitId;

            // Function Code (0x06 za single register)
            request[offset++] = writeParams.FunctionCode;

            // Start Address
            Buffer.BlockCopy(BitConverter.GetBytes(IPAddress.HostToNetworkOrder((short)writeParams.OutputAddress)), 0, request, offset, 2);
            offset += 2;

            // Value (ono što hoćemo da upišemo u register)
            Buffer.BlockCopy(BitConverter.GetBytes(IPAddress.HostToNetworkOrder((short)writeParams.Value)), 0, request, offset, 2);
            offset += 2;

            return request;
        }

        /// <inheritdoc />
        public override Dictionary<Tuple<PointType, ushort>, ushort> ParseResponse(byte[] response)
        {
            //TO DO: IMPLEMENT
            Dictionary<Tuple<PointType, ushort>, ushort> rijecnik = new Dictionary<Tuple<PointType, ushort>, ushort>();

            // Adresa (bajtovi 8 i 9)
            ushort address = (ushort)((response[8] << 8) | response[9]);

            // Vrednost koja je upisana (bajtovi 10 i 11)
            ushort value = (ushort)((response[10] << 8) | response[11]);

            // Upisujemo u rečnik — po definiciji PointType-a, to je ANALOG_OUTPUT
            rijecnik.Add(new Tuple<PointType, ushort>(PointType.ANALOG_OUTPUT, address), value);

            return rijecnik;
        }
    }
}