using MISA.Core.Entities;
using MISA.Core.Interfaces.Repository;
using MISA.Core.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.Core.Services
{
    public class PositionService : BaseService<Position>, IPositionService
    {
        #region DECLARE
        IPositionRepository _positionRepository;
        ServiceResult _serviceResult;
        #endregion
        #region Constructor
        public PositionService(IPositionRepository positionRepository) : base(positionRepository)
        {
            _serviceResult = new ServiceResult();
            _positionRepository = positionRepository;
        }
        #endregion
    }
}
