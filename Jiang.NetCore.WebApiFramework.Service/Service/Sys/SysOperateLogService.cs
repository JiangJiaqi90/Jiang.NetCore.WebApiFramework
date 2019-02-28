using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Jiang.NetCore.WebApiFramework
{
    public class SysOperateLogService : ISysOperateLogService
    {
        private readonly IUnitOfWork<ManageContext> _unitOfWork;
        private readonly ManageContext _db;
        public SysOperateLogService(IUnitOfWork<ManageContext> unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _db = _unitOfWork.DbContext;
        }
        public OperateResult<Sys_OperateLog> Add(Sys_OperateLog log)
        {
            _unitOfWork.GetRepository<Sys_OperateLog>().Insert(log);
            _unitOfWork.SaveChanges();
            return OperateResult<Sys_OperateLog>.Ok("成功",log);
        }
    }
}
