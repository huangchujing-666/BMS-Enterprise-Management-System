/*************************************************************************************
    * CLR版本：        4.0.30319.18063
    * 类 名 称：       personapermissondal
    * 机器名称：       WIN-9KCN5GHKKM8
    * 命名空间：       pbxdata.dal
    * 文 件 名：       personapermissondal
    * 创建时间：       2015/3/2 14:16:09
    * 作    者：       lcg
    * 说    明：
    * 修改时间：
    * 修 改 人：
*************************************************************************************/


using pbxdata.idal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pbxdata.dal
{
    public class personapermissondal : dataoperating, ipersonapermisson
    {
        /// <summary>
        /// 分配角色的菜单权限
        /// </summary>
        /// <param name="mdRole"></param>
        /// <returns></returns>
        public string AddRolePersonapermisson(List<model.personapermisson> list)
        {
            string s = string.Empty;

            model.pbxdatasourceDataContext context = new model.pbxdatasourceDataContext(Maticsoft.DBUtility.PubConstant.ConnectionString);
            try
            {
                context.personapermisson.InsertAllOnSubmit(list);
                context.SubmitChanges();
                s = "分配权限成功";
            }
            catch { s = "分配权限失败"; }
            return s;
        }


        /// <summary>
        /// 查询当前用户所在页面中存在哪些有权限的字段
        /// </summary>
        /// <param name="roleid">角色ID</param>
        /// <param name="menuid">菜单ID</param>
        /// <param name="funid">功能ID</param>
        /// <returns></returns>
        public int[] selectFiledPersonaPermisson(int roleid, int menuid, int funid)
        {
            int[] temp;
            model.pbxdatasourceDataContext context = new model.pbxdatasourceDataContext();
            var p = (from c in context.personapermisson where c.personaId == roleid && c.MemuId == menuid && c.FunId == funid select c.FieldId).ToList();
            temp = new int[p.Count];
            for (int i = 0; i < p.Count; i++)
            {
                if (p[i]!=null)
                {
                    temp[i] = p[i].Value;
                }
                
            }

            return temp;
        }


        /// <summary>
        /// 查询当前用户所在页面中存在哪些有权限的功能
        /// </summary>
        /// <param name="roleid">角色ID</param>
        /// <param name="menuid">菜单ID</param>
        /// <returns></returns>
        public int[] selectFunPersonaPermisson(int roleid, int menuid)
        {
            int[] temp;
            model.pbxdatasourceDataContext context = new model.pbxdatasourceDataContext();
            var p = (from c in context.personapermisson where c.personaId == roleid && c.MemuId == menuid  group c by c.FunId into g select g.Key).ToList();
            temp = new int[p.Count];

            for (int i = 0; i < p.Count; i++)
            {
                if (p[i] != null)
                {
                    temp[i] = p[i].Value;
                }
            }


            return temp;
        }


        #region 数据库中是否存在
        /// <summary>
        /// 查看权限表中是否已经记录了菜单权限
        /// </summary>
        /// <param name="roleid"></param>
        /// <param name="menuid"></param>
        /// <returns></returns>
        public bool selectMenuIsExists(int roleid, int menuid)
        {
            bool flag = false;

            model.pbxdatasourceDataContext context = new model.pbxdatasourceDataContext();
            var p = (from c in context.personapermisson where c.personaId == roleid && c.MemuId == menuid select c).FirstOrDefault();
            if (p != null)
            {
                flag = true;
            }

            return flag;
        }

        /// <summary>
        /// 查看权限表中是否已经记录了功能权限
        /// </summary>
        /// <param name="roleid"></param>
        /// <param name="menuid"></param>
        /// <param name="funid"></param>
        /// <returns></returns>
        public bool selectFunIsExists(int roleid, int menuid, int funid)
        {
            bool flag = false;

            model.pbxdatasourceDataContext context = new model.pbxdatasourceDataContext();
            var p = (from c in context.personapermisson where c.personaId == roleid && c.MemuId == menuid && c.FunId == funid select c).FirstOrDefault();
            if (p != null)
            {
                flag = true;
            }

            return flag;
        }

        /// <summary>
        /// 查看权限表中是否已经记录了字段权限
        /// </summary>
        /// <param name="roleid"></param>
        /// <param name="menuid"></param>
        /// <param name="funid"></param>
        /// <param name="filed"></param>
        /// <returns></returns>
        public bool selectFiledIsExists(int roleid, int menuid, int funid, int filed)
        {
            bool flag = false;

            model.pbxdatasourceDataContext context = new model.pbxdatasourceDataContext();
            var p = (from c in context.personapermisson where c.personaId == roleid && c.MemuId == menuid && c.FunId == funid && c.FieldId == filed select c).FirstOrDefault();
            if (p != null)
            {
                flag = true;
            }

            return flag;
        }
        #endregion

        #region 返回数据库存在的实体
        /// <summary>
        /// 查看权限表中是否已经记录了菜单权限
        /// </summary>
        /// <param name="roleid"></param>
        /// <param name="menuid"></param>
        /// <returns></returns>
        public model.personapermisson selectMenuIsExists1(int roleid, int menuid)
        {
            model.pbxdatasourceDataContext context = new model.pbxdatasourceDataContext();
            var p = (from c in context.personapermisson where c.personaId == roleid && c.MemuId == menuid select c).FirstOrDefault();
            return p;
        }

        /// <summary>
        /// 查看权限表中是否已经记录了功能权限
        /// </summary>
        /// <param name="roleid"></param>
        /// <param name="menuid"></param>
        /// <param name="funid"></param>
        /// <returns></returns>
        public model.personapermisson selectFunIsExists1(int roleid, int menuid, int funid)
        {
            model.pbxdatasourceDataContext context = new model.pbxdatasourceDataContext();
            var p = (from c in context.personapermisson where c.personaId == roleid && c.MemuId == menuid && c.FunId == funid select c).FirstOrDefault();
            return p;
        }


        /// <summary>
        /// 查看权限表中是否已经记录了字段权限
        /// </summary>
        /// <param name="roleid"></param>
        /// <param name="menuid"></param>
        /// <param name="funid"></param>
        /// <param name="filed"></param>
        /// <returns></returns>
        public model.personapermisson selectFiledIsExists1(int roleid, int menuid, int funid, int filed)
        {
            model.pbxdatasourceDataContext context = new model.pbxdatasourceDataContext();
            var p = (from c in context.personapermisson where c.personaId == roleid && c.MemuId == menuid && c.FunId == funid && c.FieldId == filed select c).FirstOrDefault();

            return p;
        }
        #endregion


        /// <summary>
        /// 撤销角色的菜单权限
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public string DelRolePersonapermisson(List<model.personapermisson> list)
        {
            string s = string.Empty;

            model.pbxdatasourceDataContext context = new model.pbxdatasourceDataContext(Maticsoft.DBUtility.PubConstant.ConnectionString);
            foreach (model.personapermisson item in list)
            {
                //var p = (list.Where(c => c.Id == item.Id)).SingleOrDefault();
                var p = (from c in context.personapermisson where c.Id == item.Id select c).SingleOrDefault();
                //context.personapermisson.Attach(p,false);
                context.personapermisson.DeleteOnSubmit(p);
            }

            try
            {
                context.SubmitChanges(System.Data.Linq.ConflictMode.FailOnFirstConflict);
                s = "撤销权限成功";
            }
            catch { s = "撤销权限失败"; }
            return s;
        }

    }
}
