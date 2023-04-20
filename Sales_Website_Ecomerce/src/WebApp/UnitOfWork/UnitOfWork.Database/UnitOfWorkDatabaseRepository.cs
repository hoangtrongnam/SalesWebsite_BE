﻿using Repository.Implement;
using Repository.Interface;
using System.Data.SqlClient;
using UnitOfWork.Interface;

namespace UnitOfWork.Database
{
    public class UnitOfWorkDatabaseRepository : IUnitOfWorkRepository
    {
        public IProductRepository ProductRepository { get; }
        public IAccountRepository AccountRepository { get; }
        public ICategoryRepository CategoryRepository { get; }
        public ICartRepository CartRepository { get; }

        public UnitOfWorkDatabaseRepository(SqlConnection context, SqlTransaction transaction)
        {          
            ProductRepository = new ProductRepository(context, transaction);
            AccountRepository = new AccountRepository(context, transaction);
            CategoryRepository = new CategoryRepository(context, transaction);
            CartRepository = new CartRepository(context, transaction);
        }
    }
}
