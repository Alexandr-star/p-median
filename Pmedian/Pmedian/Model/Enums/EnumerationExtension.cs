using System;
using System.ComponentModel;
using System.Linq;
using System.Windows.Markup;

namespace Pmedian.Model.Enums
{
    /// <summary>
    /// Вспомогательные инструменты для связи значений перечисляемых типов и их дополнительных полей.
    /// </summary>
    public class EnumerationExtension : MarkupExtension
    {
        /// <summary>
        /// Класс, определяющий перечисляемый тип.
        /// </summary>
        private Type _enumType;

        /// <summary>
        /// Конструктор с параметром.
        /// </summary>
        /// <param name="enumType">Класс, определяющий перечисляемый тип.</param>
        public EnumerationExtension(Type enumType)
        {
            EnumType = enumType ?? throw new ArgumentNullException();
        }

        /// <summary>
        /// Класс, определяющий перечисляемый тип.
        /// </summary>
        public Type EnumType
        {
            get
            {
                return _enumType;
            }
            private set
            {
                if (_enumType == value)
                    return;

                var enumType = Nullable.GetUnderlyingType(value) ?? value;

                if (enumType.IsEnum == false)
                    throw new ArgumentException();

                _enumType = value;
            }
        }

        /// <summary>
        /// Метод, генерирующий список пар значений и описаний каждого элемента перечисляемого типа.
        /// </summary>
        /// <param name="serviceProvider"></param>
        /// <returns>Список пар значений и описаный каждого элемента перечисляемого типа.</returns>
        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            var enumValues = Enum.GetValues(EnumType);

            return (
                from object enumValue in enumValues
                select new EnumerationMember
                {
                    Value = enumValue,
                    Description = GetDescription(enumValue)
                }).ToArray();
        }

        /// <summary>
        /// Метод, позволяющий получить информацию из поля описания текущего значения перечисляемого типа.
        /// </summary>
        /// <param name="enumValue">Значение перечисляемого типа.</param>
        /// <returns>Описание заданного значения перечисляемого типа.</returns>
        private string GetDescription(object enumValue)
        {
            var descriptionAttribute = EnumType
                .GetField(enumValue.ToString())
                .GetCustomAttributes(typeof(DescriptionAttribute), false)
                .FirstOrDefault() as DescriptionAttribute;

            return descriptionAttribute != null
                ? descriptionAttribute.Description
                : enumValue.ToString();
        }

        /// <summary>
        /// Класс для связи значений и данных вспомогательных полей перечисляемых типов.
        /// </summary>
        public class EnumerationMember
        {
            /// <summary>
            /// Описание значения перечисляемого типа.
            /// </summary>
            public string Description
            {
                get;
                set;
            }

            /// <summary>
            /// Текущее значение перечисляемого типа.
            /// </summary>
            public object Value
            {
                get;
                set;
            }
        }
    }
}
