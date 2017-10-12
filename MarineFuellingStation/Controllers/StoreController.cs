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
    public class StoreController : ControllerBase
    {
        private readonly StoreRepository r;
        public StoreController(StoreRepository repository)
        {
            r = repository;
        }
        [HttpGet]
        public ResultJSON<List<Store>> Get()
        {
            return new ResultJSON<List<Store>>
            {
                Code = 0,
                Data = r.GetAllList().OrderBy(s => s.Name).ToList()
            };
        }
        [HttpGet("{id}")]
        public ResultJSON<Store> Get(int id)
        {
            Store s = r.Get(id);
            return new ResultJSON<Store>
            {
                Code = 0,
                Data = s
            };
        }
        [HttpGet("[action]")]
        public ResultJSON<List<Store>> GetByStoreType(int stypeId)
        {
            return new ResultJSON<List<Store>>
            {
                Code = 0,
                Data = r.GetAllList(s => s.StoreTypeId == stypeId).OrderBy(s => s.Name).ToList()
            };
        }
        /// <summary>
        /// 根据油仓类型获取数据
        /// </summary>
        /// <param name="sc">销售仓/仓储仓</param>
        /// <returns></returns>
        [HttpGet("[action]")]
        public ResultJSON<List<Store>> GetByClass(StoreClass sc)
        {
            return new ResultJSON<List<Store>>
            {
                Code = 0,
                Data = r.GetByClass(sc)
            };
        }
        [HttpPost]
        public ResultJSON<Store> Post([FromBody]Store model)
        {
            r.CurrentUser = UserName;
            return new ResultJSON<Store>
            {
                Code = 0,
                Data = r.Insert(model)
            };
        }
        [HttpPut]
        public ResultJSON<Store> Put([FromBody]Store model)
        {
            r.CurrentUser = UserName;
            return new ResultJSON<Store>
            {
                Code = 0,
                Data = r.InsertOrUpdate(model)
            };
        }
    }
}
