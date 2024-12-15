



using TonSdk.Core.Crypto;
using TonSdk.Contracts.Wallet;
using TonSdk.Client;
using System.Xml.Linq;
using TonSdk.Core.Boc;
using TonSdk.Core.Block;
using TonSdk.Core;
using System.Threading.Tasks;
using TonSdkConsole;
using  System.Text.Json;
using System.Reflection;

internal class Program
{
    private static async Task Main(string[] args)
    {

        string MyTestWalletAdress = "create by !testnet! Tonkeeper or similar";
        string ApiKey = "take from https://testnet.toncenter.com";


        // convert from string to Address
        Address destination = new Address("EQChHpu8-rFBQyVCXJtT1aTwODTBc1dFUAEatbYy11ZLcBST"); //example address for get  nft by transaction
        Address myTestWalletAddress = new Address(MyTestWalletAdress);



        string[] words = new string[]
        {
            "24 mnemonic words" // to confirm work with your wallet
        };

        Mnemonic mnemonic = new Mnemonic(words);
        // Получение пары ключей
        KeyPair keys = mnemonic.Keys;

        // Извлечение приватного и публичного ключа
        byte[] privateKey = keys.PrivateKey;
        byte[] publicKey = keys.PublicKey;


        // Вывод ключей в консоль если вам это нужно
        Console.WriteLine("Private Key: " + BitConverter.ToString(privateKey).Replace("-", ""));
        Console.WriteLine("Public Key: " + BitConverter.ToString(publicKey).Replace("-", ""));


        WalletV4Options optionsV4 = new WalletV4Options()
        {
            PublicKey = mnemonic.Keys.PublicKey
        };

        WalletV4 walletV4R2 = new WalletV4(optionsV4, 2);
        walletV4R2.CreateDeployMessage();

        var config = new HttpParameters
        {
            Endpoint = "https://testnet.toncenter.com/api/v2/jsonRPC",
            ApiKey = ApiKey,
            Timeout = 3000
        };

        var tonClient = new TonClient(TonClientType.HTTP_TONCENTERAPIV2, config);

        // Проверка, задеплоен ли контракт / кошелёк 
        WalletFunk walletFunk = new WalletFunk(tonClient);
        walletFunk.ShowAddressInfo(myTestWalletAddress);  //  работает
        walletFunk.isWalletDeployed(MyTestWalletAdress);  //  работает принимает строку 
        walletFunk.isWalletDeployedParam(myTestWalletAddress);//  работает выводит параметор деплоя (true \ false)

        Console.ReadKey();
       
    }
}