import json

import pyodbc


class DatabaseMSSQL:
    """
    Handles the access to the database.
    """
    __db = pyodbc
    __connection_string = str

    def __init__(self):
        with open('WebCrawler/database.config.json') as config:
            db_config = json.load(config)

            self.__initialize_db_context(db_config["DB"])

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
        if context["Environment"] == "Local":
            local_obj = context["Local"]
            self.__connection_string = f'DRIVER={local_obj["Driver"]};' \
                                       f'SERVER={local_obj["Server"]};' \
                                       f'Trusted_Connection={local_obj["Trusted_Connection"]}'
        elif context["Environment"] == "Production":
            prod_obj = context["Production"]
            self.__connection_string = f'DRIVER={prod_obj["Driver"]};' \
                                       f'SERVER={prod_obj["Server"]};' \
                                       f'UID={prod_obj["UID"]};' \
                                       f'PWD={prod_obj["PWD"]}' \
                                       f'Trusted_Connection={prod_obj["Trusted_Connection"]}'
