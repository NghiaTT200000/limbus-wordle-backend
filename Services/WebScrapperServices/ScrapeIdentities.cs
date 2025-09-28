using System.Web;
using HtmlAgilityPack;
using Limbus_wordle_backend.Models;
using Limbus_wordle_backend.Util.Environment;
using Limbus_wordle_backend.Util.Functions.FileUpload;

namespace Limbus_wordle_backend.Services.WebScrapperServices
{
    public class ScrapeIdentitiesService(IdentityFileService identityFileService)
    {
        private IdentityFileService _identityFileService { get; set; } = identityFileService;
        public async Task ScrapAsync()
        {
            var web = new HtmlWeb();
            var document = web.Load("https://www.prydwen.gg/limbus-company/identities");

            var rootLink = Directory.GetCurrentDirectory();
            var filePath = Path.Combine(rootLink, EnvironmentVariables.identitiesFilePath);

            var identities = await _identityFileService.getAllIdentities();


            //Get the links to all identities
            var identitiesLinks= document.DocumentNode.QuerySelectorAll(".employees-container .avatar-card.card a")
                .Select(n=>"https://www.prydwen.gg"+n.Attributes["href"].Value)
                .ToList();
            foreach(var link in identitiesLinks)
            {
                if(!identities.ContainsKey(link))
                {
                    Console.WriteLine("Entering: "+link);
                    document = web.Load(link);
                    var characterHeader = document.DocumentNode.QuerySelector(".character-header.ag");
                    var NameNodeInside = HttpUtility.HtmlDecode(characterHeader.QuerySelector(".character-details>h1").GetDirectInnerText().Replace("NEW!"," ").Trim());
                    var Name = NameNodeInside.Replace("[","").Replace("]","").Trim();
                    var IdentityIconNode = characterHeader.QuerySelector("img[loading='lazy']");
                    var IdentityIconUrl =(IdentityIconNode!=null)?"https://www.prydwen.gg" + IdentityIconNode.Attributes["data-src"].Value:"Missing";
                    var Sinner = NameNodeInside.Split("] ")[1];
                    var skills = document.DocumentNode.QuerySelectorAll(".skills-v2 .col").Take(3);


                    List<Skill> IdentitySkills = [.. skills.Select(skill=>new Skill()
                        {
                            SinAffinity = skill.QuerySelector(".skill-header .skill-info .skill-type.pill.limbus-affinity-box").InnerText,
                            AttackType = skill.QuerySelector(".additional-information p:nth-child(1) span").InnerText,
                            SkillCoinCount= Int32.Parse(skill.QuerySelector(".additional-information p:nth-child(3) span").InnerText),
                        })];
                    var identityIconFileName = "Missing";
                    if(IdentityIconNode!=null) identityIconFileName = await Upload.UploadToCloudinary(IdentityIconUrl,Name);
                    identities[link] = new Identity()
                    {
                        Name = Name,
                        Sinner = Sinner,
                        Icon =identityIconFileName,
                        Skills = IdentitySkills
                    };
                }
            }
            await _identityFileService.saveAllIdentities(identities);
        }
    }

}