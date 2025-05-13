import { DatePicker } from "@mui/x-date-pickers";
import { Control, Controller, FieldValues } from "react-hook-form";
import dayjs from "dayjs";

const ControllerDatePricker = <T extends FieldValues>({
  fieldName,
  label,
  control,
}: {
  // eslint-disable-next-line @typescript-eslint/no-explicit-any
  fieldName: any;
  label: string;
  // eslint-disable-next-line @typescript-eslint/no-explicit-any
  control: Control<T, any>;
}) => {
  return (
    <Controller
      name={fieldName}
      control={control}
      render={({ field }) => (
        <DatePicker
          {...field}
          label={label}
          value={field.value ? dayjs(field.value) : null}
          onChange={(date) => {
            field.onChange(date?.format("YYYY-MM-DD"));
          }}
        />
      )}
    />
  );
};

export default ControllerDatePricker;
