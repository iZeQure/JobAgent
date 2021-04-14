import json

import pyodbc


class DatabaseMSSQL:
    """
    Handles the access to the database.
    """
    __db = pyodbc
    __connection_string = str

    def __init__(self, app_config: object):
        self.__initialize_db_context(app_config)

    def connect(self, auto_commit: bool = False, database: str = 'JobAgentDB_v2') -> pyodbc.Cursor:
        """
        Initializes a new connection to the database.
        Args:
            auto_commit: True for auto committing the result in the db.
            database: Used to identity the right database to execute from. Default is primary DB.
        Returns:
            object: A connection object, containing the db context.
        """
        cursor: pyodbc.Cursor
        connection: pyodbc.Connection

        try:
            connection = self.__db.connect(self.__connection_string, autocommit=auto_commit, database=database)
            cursor = connection.cursor()

            return cursor
        except Exception as ex:
            raise ex

    def __initialize_db_context(self, context: object):
        db_config = context["DatabaseConnection"]

        if 'UID' in db_config and 'PWD' in db_config:
            self.__connection_string = f'DRIVER={db_config["Driver"]};' \
                                       f'SERVER={db_config["Server"]};' \
                                       f'UID={db_config["UID"]};' \
                                       f'PWD={db_config["PWD"]}' \
                                       f'TRUSTED_CONNECTION={db_config["Trusted_Connection"]}'
            return

        self.__connection_string = f'DRIVER={db_config["Driver"]};' \
                                   f'SERVER={db_config["Server"]};' \
                                   f'TRUSTED_CONNECTION={db_config["Trusted_Connection"]}'
