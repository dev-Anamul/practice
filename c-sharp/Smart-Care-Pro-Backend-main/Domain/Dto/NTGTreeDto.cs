using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/*
 * Created by   : Lion
 * Date created : 13.02.2023
 * Modified by  : 
 * Last modified: 
 * Reviewed by  :
 * Date reviewed:
 */
namespace Domain.Dto
{
    public class NTGTreeDto
    {

        public string Id { get; set; }
        public string Parent { get; set; }
        public string Text { get; set; }
        public string Icon { get; set; }
        public TreeState? State { get; set; }
        public List<NTGTreeDto> Children { get; set; }

        public NTGTreeDto()
        {
            Children = new List<NTGTreeDto>();
        }

        public static List<NTGTreeDto> BuildTree(List<NTGLevelOneDiagnosis> nTGLevelOneDiagnoses)
        {
            List<NTGTreeDto> conditionTreeDtoList = new List<NTGTreeDto>();

            foreach (var item in nTGLevelOneDiagnoses)
            {
                NTGTreeDto conditionTreeDto = new NTGTreeDto();
                conditionTreeDto.Id = "Level#1-" + item.Oid.ToString();
                conditionTreeDto.Text = item.Description;
                conditionTreeDto.Parent = "#";
                conditionTreeDtoList.Add(conditionTreeDto);

                foreach (var item2 in item.NTGLevelTwoDiagnoses)
                {
                    NTGTreeDto conditionTreeDto2 = new NTGTreeDto();
                    conditionTreeDto2.Id = "Level#2-" + item2.Oid.ToString();
                    conditionTreeDto2.Text = item2.Description;
                    conditionTreeDto2.Parent = "Level#1-" + item.Oid.ToString();
                    conditionTreeDtoList.Add(conditionTreeDto2);

                    foreach (var item3 in item2.NTGLevelThreeDiagnoses)
                    {
                        NTGTreeDto conditionTreeDto3 = new NTGTreeDto();
                        conditionTreeDto3.Id = "Level#3-" + item3.Oid.ToString();
                        conditionTreeDto3.Text = item3.Description;
                        conditionTreeDto3.Parent = "Level#2-" + item2.Oid.ToString();
                        conditionTreeDtoList.Add(conditionTreeDto3);
                    }
                }
            }

            return conditionTreeDtoList;
        }

    }
}