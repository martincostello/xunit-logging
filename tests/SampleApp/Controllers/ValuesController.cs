// Copyright (c) Martin Costello, 2018. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;

namespace SampleApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ValuesController : ControllerBase
    {
        [HttpGet]
        public ActionResult<IEnumerable<string>> Get()
        {
            return new string[] { "a", "b", "c" };
        }

        [HttpGet("{id}")]
        public ActionResult<string> Get(string id)
        {
            return "value";
        }

        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        [HttpPut("{id}")]
        public void Put(string id, [FromBody] string value)
        {
        }

        [HttpDelete("{id}")]
        public void Delete(string id)
        {
        }
    }
}
