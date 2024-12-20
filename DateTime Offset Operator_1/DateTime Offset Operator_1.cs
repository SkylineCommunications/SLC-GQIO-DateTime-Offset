namespace Skyline.DataMiner.GQI.DateTime_Offset_Operator_1
{
	using System;

	using Skyline.DataMiner.Analytics.GenericInterface;

	[GQIMetaData(Name = "DateTime Offset Operator")]
	public class MediaOpsOperator : IGQIColumnOperator, IGQIRowOperator, IGQIInputArguments
	{
		private readonly GQIColumnDropdownArgument dateTimeValueColumnArg = new GQIColumnDropdownArgument("DateTime value") { IsRequired = true, Types = new[] { GQIColumnType.DateTime } };
		private readonly GQIIntArgument dateTimeOffsetValueArg = new GQIIntArgument("DateTime Offset Value") { IsRequired = true };
		private readonly GQIStringDropdownArgument dateTimeOffsetUnitArg = new GQIStringDropdownArgument("DateTime Offset Unit", new[] { "Minute(s)", "Hour(s)", "Day(s)" }) { IsRequired = true, DefaultValue = "Hour(s)" };
		private readonly GQIStringArgument dateTimeOutputColumnNameArg = new GQIStringArgument("DateTime result column name") { IsRequired = true };

		private GQIColumn dateTimeValueColumn;
		private GQIDateTimeColumn outputColumn;
		private int dateTimeOffsetValue;
		private string dateTimeOffsetUnit;

		public GQIArgument[] GetInputArguments()
		{
			return new GQIArgument[] { dateTimeValueColumnArg, dateTimeOffsetValueArg, dateTimeOffsetUnitArg, dateTimeOutputColumnNameArg };
		}

		public OnArgumentsProcessedOutputArgs OnArgumentsProcessed(OnArgumentsProcessedInputArgs args)
		{
			dateTimeValueColumn = args.GetArgumentValue(dateTimeValueColumnArg);
			outputColumn = new GQIDateTimeColumn(args.GetArgumentValue(dateTimeOutputColumnNameArg));
			dateTimeOffsetValue = args.GetArgumentValue(dateTimeOffsetValueArg);
			dateTimeOffsetUnit = args.GetArgumentValue(dateTimeOffsetUnitArg);

			return new OnArgumentsProcessedOutputArgs();
		}

		public void HandleColumns(GQIEditableHeader header)
		{
			header.AddColumns(outputColumn);
		}

		public void HandleRow(GQIEditableRow row)
		{
			var value = row.GetValue<DateTime>(dateTimeValueColumn);

			switch (dateTimeOffsetUnit)
			{
				case "Minute(s)":
					row.SetValue(outputColumn, value.AddMinutes(dateTimeOffsetValue));
					break;

				case "Hour(s)":
					row.SetValue(outputColumn, value.AddHours(dateTimeOffsetValue));
					break;

				case "Day(s)":
					row.SetValue(outputColumn, value.AddDays(dateTimeOffsetValue));
					break;

				default:
					row.SetValue(outputColumn, value);
					break;
			}
		}
	}
}