import {
  FormControl,
  FormControlProps,
  MenuItem,
  TextField,
  TextFieldProps,
} from "@mui/material";
import { Control, Controller, FieldErrors, FieldValues } from "react-hook-form";

const ControllerSelect = <T extends FieldValues>({
  fieldName,
  label,
  props,
  options,
  control,
  errors,
  formControlProps,
}: {
  // eslint-disable-next-line @typescript-eslint/no-explicit-any
  fieldName: any;
  label: string;
  options: { value: string | number; label: string }[];
  control: Control<T>;
  errors: FieldErrors<T>;
  // eslint-disable-next-line @typescript-eslint/no-explicit-any
  props?: TextFieldProps; // You can specify the type for props if needed
  formControlProps?: FormControlProps;
}) => {
  return (
    <Controller
      name={fieldName}
      control={control}
      render={({ field }) => (
        <FormControl {...formControlProps}>
          <TextField
            {...field}
            select
            label={label}
            value={field.value || ""}
            error={!!errors?.[fieldName]}
            helperText={(errors?.[fieldName]?.message as string) || ""}
            {...props}
          >
            {options.map((option) => (
              <MenuItem key={option.value} value={option.value}>
                {option.label}
              </MenuItem>
            ))}
          </TextField>
        </FormControl>
      )}
    />
  );
};

export default ControllerSelect;
