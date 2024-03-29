﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace PortourgalAPI.Models
{
    public class User
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public string Nome { get; set; }
        public string Distrito { get; set; }
        public string Cidade { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Imagem { get; set; }
        public int Pontos { get; set; }
        public List<Publicacao> Historico { get; set; }
    }
}
