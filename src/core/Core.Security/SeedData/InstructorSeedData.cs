using Core.Security.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Security.SeedData
{
    public static class InstructorSeedData
    {
        public static List<Instructor> GetSeedData()
        {
            return new List<Instructor>()
            {
             new Instructor{ Id = new Guid("11111111-1111-1111-1111-111111111111"),Name="Hamit Mızrak",About="Merhaba, ben Hamit Mızrak. \r\n\r\nBilgisayar Mühendisiyim.\r\nYazılımın, insanlığın dijital çağının önemli bir parçası olduğunun kanısındayım. \r\nYaklaşık 15 yıldan beri yazılımla iç içeyim. Yazılıma karşı merakım, hevesim ilk gündeki gibi.\r\nÖzel sektörde master full stack developer olarak güncel son teknoloji eğitimleri ve projeler yapmaktayım.\r\nHayatım boyunca, bilgiye aç bir ruhla, sürekli öğrenmeye ve gelişmeye odaklanarak ilerlemeye seçtim ve devam ediyorum.\r\nLise, lisans ve yüksek lisansımda da matematik ve fen bilimlerine her zaman öncelik verdim.\r\n\r\nÇocukluğumdan beri, merakımın ve keşfetme tutkumun bana rehberlik etmesiyle en çok sevdiğim meslek olan yazılımı seçtim.\r\nLisansımı ve yüksek lisansımı yazılım üzerine yaptım.\r\n\r\nKendimi sadece yazılım dünyasında geliştirmedim. Keman, Gitar, Satranç, Spor alanlarında da kendimi geliştirmeye devam ediyorum.\r\n\r\nDijital dünyanın gizemli labirentlerinden biri olan yazılım teknolojisinin entelektüel yolcusu olarak tanınırım.\r\nYazılımın her zaman toplumda ve kültürde bir iz bırakacağından emindim.\r\nBugün, hem yazılım endüstrisinde bir profesyonel olarak eğitimler, projeler yapmakta hem de akademik bir araştırmacı olarak aktif olarak çalışmaktayım.\r\nHer iki alanda da yeni projelere ve keşiflere açık olmaya devam ediyorum.\r\n\r\nGüncel yazılım olarak;\r\nFrontend development, Backend development, Database, DevOps, Mobile programming, Desktop programming, Blockchain,Web service vb., Yapay zeka, Deep Learning, Machine Learning, Blockchain alanında sürdürülebilinir güncel eğitimler vermekteyim.\r\n\r\nSevgi ve saygılarımla."},
             new Instructor{ Id =  new Guid("22222222-2222-2222-2222-222222222222"),Name="Ahmet Kaya",About="Merhaba, ben Ahmet Kaya. \r\n\r\nOyun geliştirme ve yazılım alanındaki 10 yıllık tecrübemle ve eğitmenlik\r\nkonusundaki tutkumla yeni nesil yazılımcıların gelişimine katkıda bulunuyorum. 2015 yılında\r\nKetchapp firmasında Game Developer olarak başladım ve hala burada çalışmaya devam ediyorum.\r\nKetchapp’te edindiğim değerli deneyimlerle oyun dünyasına ve yazılım mühendisliğine dair derin bir\r\nbilgi birikimi kazandım.\r\n\r\n2017 yılında Udemy platformunda eğitmenlik yapmaya başladım ve birçok öğrenciye oyun geliştirme\r\nve yazılım konusundaki bilgimi aktardım. 2021 yılında ise Udemy Business kısmında eğitimlerimi\r\nvermeye devam ederek, kurumsal düzeyde de eğitimler sunmaya başladım. Eğitim içeriklerimle\r\nbirçok profesyonelin kariyerinde fark yarattım.\r\n\r\n2023 yılında Techcareer.net ile eğitmenlik kariyerime yeni bir sayfa açtım. Burada da oyun geliştirme\r\nve yazılım konularında dersler vererek, sektörün ihtiyacı olan yetenekli ve donanımlı yazılımcıların\r\nyetişmesine yardımcı oluyorum.\r\n\r\nEğitimlerimde sadece oyun geliştirme değil, aynı zamanda backend, frontend, fullstack, SQL, Unity,\r\nPHP gibi çeşitli konuları da kapsıyorum. Bu geniş yelpazedeki konularla öğrencilerimin yazılım\r\nalanında kapsamlı bir bilgiye sahip olmalarını sağlıyorum.\r\n\r\nUzmanlık alanımda edindiğim bilgi ve tecrübeleri, öğrencilerime en etkili şekilde aktararak onların\r\nkariyer yolculuklarında başarılı olmalarını sağlamak için çalışıyorum. Oyun geliştirme süreçlerinden,\r\nyazılım mühendisliği prensiplerine kadar geniş bir yelpazede eğitimler sunarak, öğrencilerimin\r\nyetkinliklerini en üst düzeye çıkarmayı hedefliyorum.\r\n\r\nEğer siz de oyun geliştirme dünyasında sağlam adımlarla ilerlemek, yazılım ve teknoloji alanında güçlü\r\nbir temel oluşturmak istiyorsanız, benimle birlikte bu yolculuğa çıkabilirsiniz."},


            };
        }
    }
}
