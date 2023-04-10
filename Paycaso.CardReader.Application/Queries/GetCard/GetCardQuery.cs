using MediatR;
using Paycaso.CardReader.Application.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Paycaso.CardReader.Application.Queries.GetCard
{
    public class GetCardQuery : IRequest<CardDto>
    {
    }
}
