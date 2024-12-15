using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using TonSdk.Client;
using TonSdk.Core;
using TonSdk.Core.Block;
using TonSdk.Contracts.Wallet;
using static Org.BouncyCastle.Math.EC.ECCurve;
using TonSdk.Core.Boc;
using TonSdk.Core.Crypto;
namespace TonSdkConsole
{
    public class WalletFunk
    {

        private readonly ITonClient _tonClient;
    

        public WalletFunk(ITonClient tonClient)
        {
            _tonClient = tonClient;
        }






        public async void isWalletDeployed(string walletAddress)
        {
            Address myAddress = new Address(walletAddress);

            try
            {
                var isDeployed = await _tonClient.IsContractDeployed(myAddress);
                if (isDeployed)
                {
                    Console.WriteLine("Wallet is Deployed");
                }
                else
                {
                    Console.WriteLine("Wallet is NOT Deployed");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

        public void isWalletDeployedParam(Address walletAddress)
        {
            var IsContractDeployedResult =  _tonClient.IsContractDeployed(walletAddress).Result;  //  работает
            string result = IsContractDeployedResult.ToString();
            Console.WriteLine("Деплой = " + result);
        }

       public void ShowAddressInfo(Address WalletAdress) {
        var result = _tonClient.GetAddressInformation(WalletAdress).Result;

        if (result.HasValue)
        {
            AddressInformationResult addressInfo = result.Value;

        // Получаем все свойства структуры
        var properties = addressInfo.GetType().GetFields(BindingFlags.Public | BindingFlags.Instance);

            foreach (var property in properties)
            {
                string propertyName = property.Name;  // Имя свойства
        object propertyValue = property.GetValue(addressInfo);  // Значение свойства

                if (propertyValue != null)
                {
                    Console.WriteLine($"{propertyName}: {propertyValue}");
                }
                else
                {
                    Console.WriteLine($"{propertyName}: null");
                }
            }
        }
        else
        {
    Console.WriteLine("Информация об адресе отсутствует.");
        }
        }

        public async void CreateTransferMessage(WalletV4 walletV4, Mnemonic mnemonic ,Address AddressDestination ,double amount, HttpParameters config) 
        {

            var tonClient = new TonClient(TonClientType.HTTP_TONCENTERAPIV2, config);
            Coins Amount = new Coins(amount); // 1 TON
            string coinsNanoStr = new Coins("0.05").ToNano(); // "10230000000"
            string comment = "Hello TON!";


            // create transaction body query + comment
            Cell body = new CellBuilder().StoreUInt(0, 32).StoreString(comment).Build();

            // getting seqno using tonClient
            uint? seqno = await tonClient.Wallet.GetSeqno(walletV4.Address);

            //create transfer message
            ExternalInMessage message = walletV4.CreateTransferMessage(new[]
                   {
                new WalletTransfer
                {
                    Message = new InternalMessage(new InternalMessageOptions
                    {
                        Info = new IntMsgInfo(new IntMsgInfoOptions
                        {
                            Dest = AddressDestination,
                            Value = Amount,
                            Bounce = false // make bounceable message
                        }),
                        Body = body
                    }),
                    Mode = 1 // message mode сохраняем сообщение 
                }
           }, 1); // if seqno is null we pass 0, wallet will auto deploy on message send

            // sign transfer message
            message.Sign(mnemonic.Keys.PrivateKey);

            // get message cell
            Cell cell = message.Cell;

            //send this message via TonClient,
            // for example,
            // await tonClient.SendBoc(cell);
             //получение информации
        }


    }
}



