using System.Numerics;
using System.Threading.Tasks;
using Nethereum.Web3;
using Nethereum.Contracts;
using Nethereum.Hex.HexTypes;
using Nethereum.Web3.Accounts;
using Nethereum.RPC.Eth.DTOs;
using Newtonsoft.Json;
using System.Text;
using System;
using System.Security.Cryptography;

namespace BlockChain_Invoice.Services
{
    public class SaleInvoiceService
    {
        private readonly string _url= "http://127.0.0.1:7545/";
        private readonly string _abi = @"
        [
    {
      ""inputs"": [
        {
          ""internalType"": ""string"",
          ""name"": ""invoiceId"",
          ""type"": ""string""
        },
        {
          ""internalType"": ""string"",
          ""name"": ""ipfsHash"",
          ""type"": ""string""
        }
      ],
      ""name"": ""addOrUpdateSaleInvoice"",
      ""outputs"": [],
      ""stateMutability"": ""nonpayable"",
      ""type"": ""function""
    },
    {
      ""inputs"": [
        {
          ""internalType"": ""string"",
          ""name"": ""invoiceId"",
          ""type"": ""string""
        }
      ],
      ""name"": ""getSaleInvoice"",
      ""outputs"": [
        {
          ""internalType"": ""string"",
          ""name"": """",
          ""type"": ""string""
        }
      ],
      ""stateMutability"": ""view"",
      ""type"": ""function"",
      ""constant"": true
    }
  ]
    ";
        private readonly string _contractAddress = "0xb420067003D41CBC328A9e6FAa930F238EBEEcD8";
        private readonly Web3 _web3;
        private readonly Contract _contract;

        public SaleInvoiceService()
        {
            _web3 = new Web3(_url);
            _contract = _web3.Eth.GetContract(_abi, _contractAddress);
        }

        public async Task<string> AddOrUpdateSaleInvoice(string invoiceId, string ipfsHash)
        {
            try
            {
                var privateKey = "0x1c57dd549b4d9e4fb820ffaaa368423b5cc1ac5e6162d3d04f9e655109a946ef"; // Replace with your private key
                var account = new Account(privateKey);
                var addOrUpdateFunction = _contract.GetFunction("addOrUpdateSaleInvoice");
                var encodedParameters = addOrUpdateFunction.GetData(invoiceId, ipfsHash);

                var transactionInput = new TransactionInput
                {
                    To = _contractAddress,
                    From = account.Address,
                    Data = encodedParameters,
                    GasPrice = new HexBigInteger(1000000000),
                    Gas = new HexBigInteger(300000),
                    Value = new HexBigInteger(0),
                };

                var transactionHash = await _web3.Eth.TransactionManager.SendTransactionAsync(transactionInput);
                return transactionHash;
            }
            catch (System.Exception ex)
            {
                return ex.Message;
            }
        }

        public async Task<string> GetSaleInvoice(string invoiceId)
        {
            try
            {
                var getFunction = _contract.GetFunction("getSaleInvoice");
                var result = await getFunction.CallAsync<string>(invoiceId);
                return result;
            }
            catch (System.Exception ex)
            {
                return ex.Message;
            }
        }
        public static string CreateShortString(string longString)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                // Convert the long string to bytes using UTF-8 encoding
                byte[] longStringBytes = Encoding.UTF8.GetBytes(longString);

                // Compute the hash
                byte[] hashBytes = sha256.ComputeHash(longStringBytes);

                // Convert the hash bytes to a shorter representation (e.g., hex string)
                string shortString = BitConverter.ToString(hashBytes).Replace("-", "").ToLower();

                return shortString;
            }
        }

    }

    
}
