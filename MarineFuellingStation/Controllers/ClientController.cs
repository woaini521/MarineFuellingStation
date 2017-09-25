﻿using MFS.Controllers.Attributes;
using MFS.Models;
using MFS.Repositorys;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
        [HttpPost]
        public ResultJSON<Client> Post([FromBody]Client model)
        {
            r.CurrentUser = UserName;
            return new ResultJSON<Client>
            {
                Code = 0,
                Data = r.Insert(model)
            };
        }
        [HttpGet("[action]/{id}")]
        public ResultJSON<Client> GetDetail(int id)
        {
            return new ResultJSON<Client>
            {
                Code = 0,
                Data = r.GetDetail(id)
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
    }
}
