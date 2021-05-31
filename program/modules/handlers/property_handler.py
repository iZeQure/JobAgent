from past.builtins import unicode


class PropertyHandler(object):
    @staticmethod
    def is_type_of(property_type, data_type: type):
        """
        Check if the property type is type of the given type.
        Args:
            property_type: The property type to be used.
            data_type: Given data type to be checked against.

        Returns:
            True if the given type is type of data type, else False.

        """
        if isinstance(type(property_type), (data_type, unicode)):
            return True
        else:
            return False

        # return type(property_type) == data_type
