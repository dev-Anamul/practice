﻿using Domain.Entities;
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
    public class ICDsDigonosisCodeDto
    {
        public string Id { get; set; }
        public string Parent { get; set; }
        public string Text { get; set; }
        public string Icon { get; set; }
        public TreeState? State { get; set; }
        public List<ICDsDigonosisCodeDto> Children { get; set; }

        public ICDsDigonosisCodeDto()
        {
            Children = new List<ICDsDigonosisCodeDto>();
        }

        public static List<ICDsDigonosisCodeDto> BuildTree(List<ICDsDigonosisCodeDto> list, string parentId = "#")
        {
            var workingList = list
                .Where(q => q.Parent.Equals(parentId))
                .OrderBy(q => q.Text)
                .ToList();

            if (!workingList.Any())
                return new List<ICDsDigonosisCodeDto>();

            var listTree = new List<ICDsDigonosisCodeDto>();

            foreach (var treeItem in workingList.Select(item => new ICDsDigonosisCodeDto
            {
                Id = item.Id,
                Parent = item.Parent,
                Text = item.Text,
                Icon = "false",
                Children = BuildTree(list, item.Id)
            }))
            {
                if (treeItem.Children.Any())
                {
                    treeItem.State ??= new TreeState();
                    treeItem.State.Disabled = true;
                }

                listTree.Add(treeItem);
            }

            return listTree;
        }

        public static List<ICDsDigonosisCodeDto> BuildTree(List<ICDDiagnosis> icdDiagnosisCodes)
        {
            var list = icdDiagnosisCodes.Select(item => new ICDsDigonosisCodeDto
            {
                Id = item.Oid.ToString(),
                Parent = item.ParentId == 0 ? "#" : item.ParentId.ToString(),
                Text = item.ICDCode + " - " + item.Description,
            }).ToList();

            return list;
        }


    }

    public class TreeState
    {
        public bool? Opened { get; set; }
        public bool? Disabled { get; set; }
        public bool? Selected { get; set; }
    }
}
