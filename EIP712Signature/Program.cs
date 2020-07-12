using Nethereum.Hex.HexConvertors.Extensions;
using Nethereum.Signer;
using Nethereum.Signer.EIP712;
using Nethereum.Web3.Accounts;
using System;
using System.Collections.Generic;

namespace EIP712Signature
{
    class Program
    {
        static void Main(string[] args)
        {
            var privKey = "0x83f8964bd55c98a4806a7b100bd9d885798d7f936f598b88916e11bade576204";
            var account = new Account(privKey);
            var typedData = new TypedData
            {
                Domain = new Domain
                {
                    Name = "Ether Mail",
                    Version = "1",
                    ChainId = 1,
                    VerifyingContract = "0xCcCCccccCCCCcCCCCCCcCcCccCcCCCcCcccccccC"
                },
                Types = new Dictionary<string, MemberDescription[]>
                {
                    ["EIP712Domain"] = new[]
                    {
                        new MemberDescription {Name = "name", Type = "string"},
                        new MemberDescription {Name = "version", Type = "string"},
                        new MemberDescription {Name = "chainId", Type = "uint256"},
                        new MemberDescription {Name = "verifyingContract", Type = "address"},
                    },
                    ["Person"] = new[]
                    {
                        new MemberDescription {Name = "name", Type = "string"},
                        new MemberDescription {Name = "wallet", Type = "address"},
                    },
                    ["Mail"] = new[]
                    {
                        new MemberDescription {Name = "from", Type = "Person"},
                        new MemberDescription {Name = "to", Type = "Person"},
                        new MemberDescription {Name = "contents", Type = "string"},
                    }
                },
                PrimaryType = "Mail",
                Message = new[]
                {
                    new MemberValue
                    {
                        TypeName = "Person", Value = new[]
                        {
                            new MemberValue {TypeName = "string", Value = "Cow"},
                            new MemberValue {TypeName = "address", Value = "0xCD2a3d9F938E13CD947Ec05AbC7FE734Df8DD826"},
                        }
                    },
                    new MemberValue
                    {
                        TypeName = "Person", Value = new[]
                        {
                            new MemberValue {TypeName = "string", Value = "Bob"},
                            new MemberValue {TypeName = "address", Value = "0xbBbBBBBbbBBBbbbBbbBbbbbBBbBbbbbBbBbbBBbB"},
                        }
                    },
                    new MemberValue {TypeName = "string", Value = "Hello, Bob!"},
                }
            };

            var sig = Eip712TypedDataSigner.Current.SignTypedData(typedData, new EthECKey(account.PrivateKey));
            var parsedSignature = ParseSignature(sig);
            Console.WriteLine(
                $"signature:\n{sig}\n" +
                $"SigR:\n{parsedSignature.R}\n" +
                $"SigS:\n{parsedSignature.S}\n" +
                $"SigV:\n{parsedSignature.V}\n"
                );
        }

        private static Signature ParseSignature (string fullSig)
        {
            if (fullSig.StartsWith("0x"))
                fullSig = fullSig[2..];
            return new Signature
            {
                R = "0x" + fullSig.Substring(0, 64),
                S = "0x" + fullSig.Substring(64, 64),
                V = Convert.ToByte(fullSig.Substring(128, 2), 16)
            };
        }
    }
}
