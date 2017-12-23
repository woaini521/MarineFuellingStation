﻿using MFS.Controllers.Attributes;
using MFS.Models;
using MFS.Repositorys;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace MFS.Controllers
{
    [Route("api/[controller]"), Axios]
    public class ClientController : ControllerBase
    {
        private readonly ClientRepository r;
        public ClientController(ClientRepository repository)
        {
            r = repository;
        }
        #region POST
        [HttpPost]
        public ResultJSON<Client> Post([FromBody]Client model)
        {
            if (r.Has(c => c.CarNo == model.CarNo)) return new ResultJSON<Client> { Code = 0, Msg = "操作失败，已存在" + model.CarNo };
            r.CurrentUser = UserName;
            return new ResultJSON<Client>
            {
                Code = 0,
                Data = r.Insert(model)
            };
        }
        #endregion
        #region GET
        [HttpGet("[action]/{id}")]
        public ResultJSON<Client> GetDetail(int id)
        {
            return new ResultJSON<Client>
            {
                Code = 0,
                Data = r.GetDetail(id)
            };
        }
        /// <summary>
        /// 根据车号/船号获得客户
        /// </summary>
        /// <param name="carNo"></param>
        /// <returns></returns>
        [HttpGet("[action]")]
        public ResultJSON<Client> CreateOrGetClientByCarNo(string carNo)
        {
            r.CurrentUser = UserName;
            return new ResultJSON<Client>
            {
                Code = 0,
                Data = r.CreateOrGetByCarNo(carNo)
            };
        }
        [HttpGet]
        public ResultJSON<List<Client>> Get()
        {
            List<Client> ls = r.GetIncludeCompany();
            return new ResultJSON<List<Client>>
            {
                Code = 0,
                Data = ls
            };
        }
        [HttpGet("{sv}")]
        public ResultJSON<List<Client>> Get(string sv)
        {
            List<Client> ls = r.GetIncludeCompany(sv);
            return new ResultJSON<List<Client>>
            {
                Code = 0,
                Data = ls
            };
        }
        /// <summary>
        /// 只根据client表内字段搜索关键字
        /// </summary>
        /// <param name="kw">电话|联系人|船号|车号关键字</param>
        /// <returns></returns>
        [HttpGet("[action]")]
        public ResultJSON<List<Client>> GetByClientKeyword(string kw)
        {
            List<Client> ls = r.GetByClientKeyword(kw);
            return new ResultJSON<List<Client>>
            {
                Code = 0,
                Data = ls
            };
        }
        /// <summary>
        /// 根据条件查询数据
        /// </summary>
        /// <param name="ctype">客户类型</param>
        /// <param name="ptype">计划状态</param>
        /// <param name="balances">余额条件</param>
        /// <param name="cycle">周期条件</param>
        /// <param name="kw">搜索关键字</param>
        [HttpGet("[action]")]
        public ResultJSON<List<Client>> GetClients(ClientType ctype, int ptype, int balances, int cycle, string kw, bool isMy)
        {
            r.CurrentUser = UserName;
            return new ResultJSON<List<Client>>
            {
                Code = 0,
                Data = r.GetMyClients(ctype, ptype, balances, cycle, kw, isMy)
            };
        }
        #endregion
        #region PUT
        /// <summary>
        /// 标记客户
        /// </summary>
        /// <param name="c">客户model</param>
        [HttpPut("[action]")]
        public ResultJSON<Client> MarkTag([FromBody]Client c)
        {
            r.CurrentUser = UserName;
            return new ResultJSON<Client>
            {
                Code = 0,
                Data = r.Update(c)
            };
        }
        /// <summary>
        /// 清除我的客户所有标注
        /// </summary>
        /// <param name="c">客户model</param>
        [HttpPut("[action]")]
        public ResultJSON<Client> ClearMyClientMark()
        {
            r.CurrentUser = UserName;
            return new ResultJSON<Client>
            {
                Code = 0,
                Msg = "成功更新了" + r.Update(c => c.FollowSalesman == UserName,
                new Client()
                {
                    IsMark = false
                }).ToString() + "条信息"
            };
        }
        /// <summary>
        /// 提交客户备注信息
        /// </summary>
        /// <param name="c">客户model</param>
        [HttpPut("[action]")]
        public ResultJSON<Client> ReMark([FromBody]Client c)
        {
            r.CurrentUser = UserName;
            return new ResultJSON<Client>
            {
                Code = 0,
                Data = r.Update(c)
            };
        }
        /// <summary>
        /// 把一个或多个客户归入到公司
        /// </summary>
        /// <param name="clientIds"></param>
        /// <param name="companyId"></param>
        /// <returns></returns>
        [HttpPut("[action]")]
        public ResultJSON<List<Client>> SetClientsToCompany(string clientIds, int companyId)
        {
            List<Client> list = r.SetClientsToCompany(clientIds.Split(','), companyId);
            if(list.Count == 0)
                return new ResultJSON<List<Client>>
                {
                    Code = 503,
                    Msg = "选择的客户有误或存在该公司"
                };
            return new ResultJSON<List<Client>>
            {
                Code = 0,
                Data = list
            };
        }
        /// <summary>
        /// 移除一个或多个客户
        /// </summary>
        /// <param name="clientIds"></param>
        /// <param name="companyId"></param>
        /// <returns></returns>
        [HttpPut("[action]")]
        public ResultJSON<List<Client>> RemoveCompanyClients(string clientIds, int companyId)
        {
            List<Client> list = r.RemoveCompanyClients(clientIds.Split(','), companyId);
            if (list.Count == 0)
                return new ResultJSON<List<Client>>
                {
                    Code = 503,
                    Msg = "选择的客户有误或存在该公司"
                };
            return new ResultJSON<List<Client>>
            {
                Code = 0,
                Data = list
            };
        }
        [HttpPut]
        public ResultJSON<Client> Save([FromBody]Client c)
        {
            return new ResultJSON<Client>
            {
                Code = 0,
                Data = r.Update(c)
            };
        }
        #endregion
    }
}
