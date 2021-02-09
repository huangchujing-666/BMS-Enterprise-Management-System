using pbxdata.idal;
using pbxdata.model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pbxdata.dal
{
    public partial class CheckProducdal : dataoperating
    {
        /// <summary>
        /// 搜索条件商品信息
        /// </summary>
        /// <returns></returns>
        public DataTable GetSearchList(Dictionary<string, string> Dic, int page, int pages, out string counts)
        {
            DataTable dt = new DataTable();
            int Minnid = pages * (page - 1);
            int Maxnid = pages;
            model.pbxdatasourceDataContext context = new pbxdatasourceDataContext();
            var q = from c in context.product
                    join ty in context.producttype on c.Cat2 equals ty.TypeNo
                    into pt
                    from ss in pt.DefaultIfEmpty()
                    join o in context.productCustomsResult on c.Scode equals o.productScode
                    into t
                    from s in t.DefaultIfEmpty()
                    select new
                    {
                        Scode = c.Scode,//货号
                        Bcode = c.Bcode,//
                        Bcode2 = c.Bcode2,
                        Descript = c.Descript,//英文描述
                        Cdescript = c.Cdescript,//中文描述
                        Unit = c.Unit,//单位
                        Currency = c.Currency,//货币
                        Cat = c.Cat,//品牌
                        Cat1 = c.Cat1,//季节
                        Cat2 = c.Cat2,//类别
                        Clolor = c.Clolor,//颜色
                        Size = c.Size,//尺寸
                        Style = c.Style,//款号
                        Vencode = c.Vencode,//供应商
                        Model = c.Model,//型号
                        Rolevel = c.Rolevel,
                        Roamt = c.Roamt,
                        Stopsales = c.Stopsales,
                        Loc = c.Loc,//店铺
                        Balance = c.Balance,//库存
                        Lastgrnd = c.Lastgrnd,//最后一次操作时间
                        Imagefile = c.Imagefile,//缩略图
                        ciqProductId = c.ciqProductId,//商品信息图  1.合格 2.未标记 3.不合格
                        ciqSpec = c.ciqSpec,//规格型号
                        ciqHSNo = c.ciqHSNo,//商品HS编码
                        ciqAssemCountry = c.ciqAssemCountry,//原产国/地区
                        ciqProductNo = c.ciqProductNo,//商品序号（订单报备EntGoodsNo字段）长度小于6
                        QtyUnit = c.QtyUnit,//计量单位	商品购买数/重量的计量单位
                        Def15 = c.Def15,//毛重
                        Def16 = c.Def16,//净重

                        SJstatus = s.SJstatus,//商品报关 订单状态(0失败，1成功，2未上传,3上传成功)
                        RegStatus = s.RegStatus,//商品报关 ICIP回执状态 最大4字符 10:通过;20:不通过
                        RegNotes = s.RegNotes,//商品报关 ICIP回执信息	最大256字符
                        BBCask = s.BBCask,//物流状态  1: 成功 0: 失败
                        BBCerrorMessage = s.BBCerrorMessage,//物流--错误信息

                        TypeName = ss.TypeName,//类别名

                    };
            //通过各个条件查询
            if (Dic["Style"] != "")
            {
                q = q.Where(a => a.Style.Contains(Dic["Style"]));
            }
            if (Dic["Scode"] != "")
            {
                q = q.Where(a => a.Scode.Contains(Dic["Scode"]));
            }
            if (Dic["Cat"] != "")
            {
                q = q.Where(a => a.Cat.Contains(Dic["Cat"]));
            }
            if (Dic["Cat1"] != "")
            {
                q = q.Where(a => a.Cat1.Contains(Dic["Cat1"]));
            }
            if (Dic["Cat2"] != "")
            {
                q = q.Where(a => a.Cat2.Contains(Dic["Cat2"]));
            }
            if (Dic["Color"] != "")
            {
                q = q.Where(a => a.Style.Contains(Dic["Color"]));
            }
            if (Dic["MinBalance"] != "")
            {
                q = q.Where(a => a.Balance >= Convert.ToInt32(Dic["MinBalance"]));
                if (Dic["MaxBalance"] != "")
                {
                    q = q.Where(a => a.Balance <= Convert.ToInt32(Dic["MaxBalance"]));
                }
            }
            else
            {
                if (Dic["MaxBalance"] != "")
                {
                    q = q.Where(a => a.Balance <= Convert.ToInt32(Dic["MaxBalance"]));
                }
            }
            if (Dic["Imagefile"] != "")
            {
                if (Dic["Imagefile"] == "0")
                {
                    q = q.Where(a => a.Imagefile == "" || a.Imagefile == null);
                }
                else if (Dic["Imagefile"] == "1")
                {
                    q = q.Where(a => a.Imagefile != "" && a.Imagefile != null);
                }
            }

            if (Dic["SJstatus"] != "")
            {

                if (Dic["SJstatus"] == "2")
                {
                    q = q.Where(a => a.SJstatus == Dic["SJstatus"] || a.SJstatus == "" || a.SJstatus == null);
                }
                else
                {
                    q = q.Where(a => a.SJstatus == Dic["SJstatus"]);
                }
            }
            if (Dic["RegStatus"] != "")
            {
                q = q.Where(a => a.RegStatus == Dic["RegStatus"]);
            }
            if (Dic["BBCask"] != "")
            {
                if (Dic["BBCask"] == "2")
                {
                    q = q.Where(a => a.BBCask == "" || a.BBCask == null);
                }
                else
                {
                    q = q.Where(a => a.BBCask == Dic["BBCask"]);
                }
            }

            if (Dic["ZLStatus"] != "")
            {
                if (Dic["ZLStatus"] == "0")
                {
                    q = q.Where(a => a.ciqSpec == "" || a.ciqSpec == null || a.ciqHSNo == "" || a.ciqHSNo == null || a.ciqAssemCountry == "" || a.ciqAssemCountry == null || a.QtyUnit == "" || a.QtyUnit == null || a.Def15 == "" || a.Def15 == null || a.Def16 == "" || a.Def16 == null);
                }
                else if (Dic["ZLStatus"] == "1")
                {
                    q = q.Where(a => a.ciqSpec != "" && a.ciqHSNo != "" && a.ciqAssemCountry != "" && a.QtyUnit != "" && a.Def15 != "" && a.Def16 != "" && a.ciqSpec != null && a.ciqHSNo != null && a.ciqAssemCountry != null && a.QtyUnit != null && a.Def15 != null && a.Def16 != null);
                }
            }

            if (Dic["ciqHSNo"] == "0")
            {
                q = q.Where(a => a.ciqHSNo == null || a.ciqHSNo == "");
            }
            else if (Dic["ciqHSNo"] == "1")
            {
                q = q.Where(a => a.ciqHSNo != null && a.ciqHSNo != "");
            }

            if (Dic["ciqAssemCountry"] == "0")
            {
                q = q.Where(a => a.ciqAssemCountry == null || a.ciqAssemCountry == "");
            }
            else if (Dic["ciqAssemCountry"] == "1")
            {
                q = q.Where(a => a.ciqAssemCountry != null && a.ciqAssemCountry != "");
            }

            if (Dic["QtyUnit"] == "0")
            {
                q = q.Where(a => a.QtyUnit == null || a.QtyUnit == "");
            }
            else if (Dic["QtyUnit"] == "1")
            {
                q = q.Where(a => a.QtyUnit != null && a.QtyUnit != "");
            }

            if (Dic["Def15"] == "0")
            {
                q = q.Where(a => a.Def15 == null || a.Def15 == "");
            }
            else if (Dic["Def15"] == "1")
            {
                q = q.Where(a => a.Def15 != null && a.Def15 != "");
            }

            if (Dic["Def16"] == "0")
            {
                q = q.Where(a => a.Def16 == null || a.Def16 == "");
            }
            else if (Dic["Def16"] == "1")
            {
                q = q.Where(a => a.Def16 != null && a.Def16 != "");
            }
            if (Dic["ciqProductId"] != "")
            {
                q = Dic["ciqProductId"] == "2" ? q.Where(a => a.ciqProductId == "" || a.ciqProductId == null) : q.Where(a => a.ciqProductId == Dic["ciqProductId"]);
            }
            q = q.OrderByDescending(a => a.Balance);//根据库存排序
            counts = q.Count().ToString();//返回查询数量
            dt = LinqToDataTable.LINQToDataTable(q.Skip(Minnid).Take(Maxnid));//翻页
            return dt;
        }
    }
}
