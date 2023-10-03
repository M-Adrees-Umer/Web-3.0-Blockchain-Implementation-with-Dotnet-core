using Nethereum.Web3;
using Nethereum.Contracts;
using System.Numerics;
using System.Threading.Tasks;
using System;
using Nethereum.Hex.HexTypes;
using Nethereum.Hex.HexConvertors.Extensions;
using Nethereum.RPC.Eth.DTOs;
using ADRaffy.ENSNormalize;
using Nethereum.Web3.Accounts;
using Nethereum.ABI.FunctionEncoding.Attributes;
using Nethereum.ABI.Model;
using Nethereum.ABI.Decoders;
namespace BlockCahin_Invoice.Services

{
    public class EthereumService
    {
        private readonly string _url = "http://127.0.0.1:7545/";
        private readonly string _abi = @"
        [
            {
                ""anonymous"": false,
                ""inputs"": [
                    {
                        ""indexed"": true,
                        ""internalType"": ""uint256"",
                        ""name"": ""id"",
                        ""type"": ""uint256""
                    },
                    {
                        ""indexed"": false,
                        ""internalType"": ""string"",
                        ""name"": ""customerName"",
                        ""type"": ""string""
                    },
                    {
                        ""indexed"": false,
                        ""internalType"": ""uint256"",
                        ""name"": ""amount"",
                        ""type"": ""uint256""
                    }
                ],
                ""name"": ""InvoiceCreated"",
                ""type"": ""event""
            },
            {
                ""inputs"": [],
                ""name"": ""invoiceCount"",
                ""outputs"": [
                    {
                        ""internalType"": ""uint256"",
                        ""name"": """",
                        ""type"": ""uint256""
                    }
                ],
                ""stateMutability"": ""view"",
                ""type"": ""function"",
                ""constant"": true
            },
            {
                ""inputs"": [
                    {
                        ""internalType"": ""uint256"",
                        ""name"": """",
                        ""type"": ""uint256""
                    }
                ],
                ""name"": ""invoices"",
                ""outputs"": [
                    {
                        ""internalType"": ""string"",
                        ""name"": ""id"",
                        ""type"": ""string""
                    },
                    {
                        ""internalType"": ""string"",
                        ""name"": ""customerName"",
                        ""type"": ""string""
                    },
                    {
                        ""internalType"": ""uint256"",
                        ""name"": ""amount"",
                        ""type"": ""uint256""
                    }
                ],
                ""stateMutability"": ""view"",
                ""type"": ""function"",
                ""constant"": true
            },
            {
                ""inputs"": [
                    {
                        ""internalType"": ""string"",
                        ""name"": ""_id"",
                        ""type"": ""string""
                    },
                    {
                        ""internalType"": ""string"",
                        ""name"": ""_customerName"",
                        ""type"": ""string""
                    },
                    {
                        ""internalType"": ""uint256"",
                        ""name"": ""_amount"",
                        ""type"": ""uint256""
                    }
                ],
                ""name"": ""createInvoice"",
                ""outputs"": [],
                ""stateMutability"": ""nonpayable"",
                ""type"": ""function""
            },
            {
                ""inputs"": [
                    {
                        ""internalType"": ""uint256"",
                        ""name"": ""_id"",
                        ""type"": ""uint256""
                    }
                ],
                ""name"": ""getInvoice"",
                ""outputs"": [
                    {
                        ""internalType"": ""string"",
                        ""name"": """",
                        ""type"": ""string""
                    },
                    {
                        ""internalType"": ""string"",
                        ""name"": """",
                        ""type"": ""string""
                    },
                    {
                        ""internalType"": ""uint256"",
                        ""name"": """",
                        ""type"": ""uint256""
                    }
                ],
                ""stateMutability"": ""view"",
                ""type"": ""function"",
                ""constant"": true
            }
        ]
    ";
        private readonly string _contractAddress = "0xe7eE81857D2600c8947f190eA78049Ad056284a8";
        private readonly Web3 _web3;
        private readonly Contract _contract;

        public EthereumService()
        {
            _web3 = new Web3(_url);
            _contract = _web3.Eth.GetContract(_abi, _contractAddress);
        }

        public async Task<string> CreateInvoice(string id, string customerName, BigInteger amount)
        {
            try
            {
                var privateKey = "0x05eec0736bee951f8658c91275cdeebb9555771c8c4f51567ab50cbdb30fca4f";
                var account = new Account(privateKey);
                var createInvoiceFunction = _contract.GetFunction("createInvoice");
                var encodedParameters = createInvoiceFunction.GetData(id, customerName, amount);

                var transactionInput = new TransactionInput
                {
                    To = _contractAddress,
                    From = account.Address,
                    Data = encodedParameters,
                    GasPrice = new HexBigInteger(90000000000),
                    Gas = new HexBigInteger(300000),
                    Value = new HexBigInteger(0),
                };
                // Estimate gas for the function call
                

                var transactionHash = await _web3.Eth.TransactionManager.SendTransactionAsync(transactionInput);
                return transactionHash;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }


        public async Task<InvoiceData> GetInvoice(uint id)
        {
            try
            {
                var getInvoiceFunction = _contract.GetFunction("getInvoice");

                // Convert uint to BigInteger
                var idBigInteger = new BigInteger(id);

                // Call the function with the idBigInteger parameter
                var result = await getInvoiceFunction.CallDeserializingToObjectAsync<InvoiceData>(idBigInteger);

                return result;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
    }
    [FunctionOutput]
    public class InvoiceData
    {
        [Parameter("string", "id", 1)]
        public string Id { get; set; }

        [Parameter("string", "customerName", 2)]
        public string CustomerName { get; set; }

        [Parameter("uint256", "amount", 3)]
        public BigInteger Amount { get; set; }
    }
}
