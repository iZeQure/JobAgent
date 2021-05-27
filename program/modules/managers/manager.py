import logging as log
from program.modules.database.db import DatabaseMSSQL
from pyodbc import Cursor, ProgrammingError


class Manager:
    __db: DatabaseMSSQL

    def __init__(self, database: DatabaseMSSQL):
        self.__db = database

    def __connect(self, db_scheme: str = None, auto_commit: bool = False) -> Cursor:
        if db_scheme is None:
            return self.__db.connect(auto_commit)

        return self.__db.connect(auto_commit, db_scheme)

    def get_sql_data(self, sp_sql: str,
                     params: [] = None,
                     db_scheme: str = None,
                     auto_commit: bool = False,
                     fetch_mode: str = 'ALL') -> []:
        """

        Args:
            sp_sql:
            params:
            db_scheme:
            auto_commit: Default is false, set to true if the data should be commited automatically in the database.
            fetch_mode: Chooses whether the result should be returned as everything or one. Can be defined setting fetch_mode to `ALL` or `ONE`.

        Returns:

        """
        with self.__connect(db_scheme, auto_commit) as conn:
            try:
                if params is None:
                    conn.execute(sp_sql)
                    return conn.fetchall()

                conn.execute(sp_sql, params)

                if fetch_mode == 'ALL':
                    return conn.fetchall()
                elif fetch_mode == 'ONE':
                    return conn.fetchone()

                return conn.fetchall()

            except ProgrammingError:
                log.error(f'Error while fetching data from <{db_scheme}>')

    def save_data(self, sp_sql: str, params: [], db_scheme: str = None, auto_commit: bool = False):
        with self.__connect(db_scheme, auto_commit) as conn:
            conn.execute(sp_sql, params)
