using Jiang.NetCore.WebApiFramework;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.EntityFrameworkCore
{
    /// <summary>
    /// 扩展查询
    /// 扩展Microsoft.EntityFrameworkCore.UnitOfWork 仓储
    /// </summary>
    public static class RepositoryExtensions
    {
        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="query">查询linq</param>
        /// <param name="predicate">筛选条件表达式树</param>
        /// <param name="orderBy">排序函数</param>
        /// <param name="include">A function to include navigation properties</param>
        /// <param name="pageIndex">The index of page.</param>
        /// <param name="pageSize">The size of the page.</param>
        /// <param name="disableTracking"><c>True</c> to disable changing tracking; otherwise, <c>false</c>. Default to <c>true</c>.</param>
        /// <returns>An <see cref="IPagedList{TEntity}"/> that contains elements that satisfy the condition specified by <paramref name="predicate"/>.</returns>
        /// <remarks>This method default no-tracking query.</remarks>
        public static IPagedList<TEntity> GetPagedList<Tin, TEntity>(this IRepository<Tin> repo, IQueryable<TEntity> query, Expression<Func<TEntity, bool>> predicate = null,
                                                Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
                                                Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null,
                                                int pageIndex = 0,
                                                int pageSize = 20,
                                                bool disableTracking = true) where Tin : class where TEntity : class
        {
            if (disableTracking)
            {
                query = query.AsNoTracking();
            }

            if (include != null)
            {
                query = include(query);
            }

            if (predicate != null)
            {
                query = query.Where(predicate);
            }

            if (orderBy != null)
            {
                return orderBy(query).ToPagedList(pageIndex, pageSize);
            }
            else
            {
                return query.ToPagedList(pageIndex, pageSize);
            }
        }
        /// <summary>
        /// 删除所有
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="repo"></param>
        public static void DeleteAll<TEntity>(this IRepository<TEntity> repo) where TEntity : class
        {
            repo.Delete(repo.GetAll());
        }
        ///// <summary>
        ///// 添加附件
        ///// </summary>
        ///// <param name="unitOfWork"></param>
        ///// <param name="attachment">附件</param>
        ///// <returns></returns>
        //public static async Task<OperateResult<Sys_Attachment>> AddAttachmentAsync(this IUnitOfWork unitOfWork, SysAttachmentParam attachment)
        //{
        //    var file = attachment.File;
        //    //文件校验
        //    var op = file.IsIllegalFile();
        //    if (op.Code != ResultCode.OK)
        //        return op;
        //    //上传的文件名
        //    var name = file.FileName;
        //    //相对路径\年\月\类型\
        //    var relationDir = string.Format("\\{0}\\{1}\\", DateTime.Now.Year, DateTime.Now.Month);
        //    //全路径。
        //    var wholeDir = string.Format("{0}{1}", GlobalParams.AttachmentBasePath, relationDir);
        //    //保存文件
        //    //获取路径，不存在就创建
        //    if (!Directory.Exists(wholeDir))
        //    {
        //        Directory.CreateDirectory(wholeDir);
        //    }
        //    //保存到服务器的文件名为GUID
        //    var fileName = $"{wholeDir}{Guid.NewGuid()}{Path.GetExtension(name)}";
        //    using (var stream = File.Create(fileName))
        //    {
        //        await file.CopyToAsync(stream);
        //    }
        //    //文件类
        //    var att = new Sys_Attachment()
        //    {
        //        Id = Guid.NewGuid(),
        //        Path = relationDir + "\\" + Path.GetFileName(fileName),
        //        FileName = Path.GetFileName(name),
        //        Rename = Path.GetFileNameWithoutExtension(name),
        //        UserId = attachment.UserId
        //    };
        //    var repo = unitOfWork.GetRepository<Sys_Attachment>();
        //    repo.Insert(att);
        //    return OperateResult<Sys_Attachment>.Ok("成功", att);
        //}
        ///// <summary>
        ///// 获取会员赠送金额委托
        ///// </summary>
        ///// <param name="db">数据上下文</param>
        ///// <returns></returns>
        //public static Func<decimal, decimal> GetGiveMoneyFunction(this HotelManageContext db)
        //{
        //    var list = db.Sys_MembersTopUpRules.OrderByDescending(l=>l.RechargePrice);
        //    Func<decimal, decimal> func = (money) =>
        //    {
        //        foreach (var l in list)
        //        {
        //            if (money >= l.RechargePrice)
        //            {
        //                return l.DepartmentId;
        //            }
        //        }

        //        return 0M;
        //    };
        //    return func;
        //}
        ///// <summary>
        ///// 获取会员等级委托
        ///// </summary>
        ///// <param name="db">数据上下文</param>
        ///// <returns></returns>
        //public static Func<int, Guid> GetMemberLevelFunction(this HotelManageContext db)
        //{
        //    var me = new Mem_Info();
        //    var list = db.Sys_MembersLevelInfo.OrderByDescending(l => l.MinConsumption);
        //    Func<int, Guid> func = (integral) =>
        //    {
        //        foreach (var l in list)
        //        {
        //            if (integral >= l.MinConsumption)
        //            {
        //                return l.Id;
        //            }
        //        }

        //        return list.Last().Id;
        //    };
        //    return func;
        //}
        ///// <summary>
        ///// 是否为非法文件
        ///// 文件名非法，文件过大（超过50M）
        ///// </summary>
        ///// <param name="file">文件</param>
        ///// <returns>true表示非法文件</returns>
        //private static OperateResult<Sys_Attachment> IsIllegalFile(this IFormFile file)
        //{
        //    if (file == null)
        //    {
        //        return OperateResult<Sys_Attachment>.Error("文件不存在");
        //    }
        //    if (string.IsNullOrEmpty(file.FileName))
        //    {
        //        return OperateResult<Sys_Attachment>.Error("文件不存在");
        //    }
        //    //超过50m不予上传
        //    var limit = 50 * 1000 * 1000;
        //    if (file.Length > limit)
        //    {
        //        return OperateResult<Sys_Attachment>.Error("文件太大，请选择小于50M的文件！");
        //    }
        //    //非法文件后缀
        //    var illegals = new List<string>() { ".exe", ".dll", ".bat" };
        //    var name = file.FileName;
        //    var aLastName = Path.GetExtension(name); ; //扩展名
        //    if (illegals.Contains(aLastName))
        //    {
        //        return OperateResult<Sys_Attachment>.Error("非法文件");
        //    }
        //    return OperateResult<Sys_Attachment>.Ok();
        //}



    }
}
