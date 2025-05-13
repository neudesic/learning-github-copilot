import {
  FormControl,
  FormControlProps,
  FormLabel,
  FormLabelProps,
  TextField,
  TextFieldProps,
} from "@mui/material";
import { Control, Controller, FieldErrors, FieldValues } from "react-hook-form";

const ControllerTextField = <T extends FieldValues>({
  fieldName,
  label,
  props,
  control,
  errors,
  formLabel,
  formControlProps,
}: {
  // eslint-disable-next-line @typescript-eslint/no-explicit-any
  fieldName: any;
  label: string;
  props: TextFieldProps;
  // eslint-disable-next-line @typescript-eslint/no-explicit-any
  control: Control<T, any>;
  errors: FieldErrors<T>;
  formLabel?: {
    name: string;
    props: FormLabelProps;
  };
  formControlProps?: FormControlProps;
}) => {
  return (
    <Controller<T>
      name={fieldName}
      control={control}
      render={({ field }) => (
        <FormControl {...formControlProps}>
          {formLabel && (
            <FormLabel
              component="legend"
              {...formLabel.props}
              sx={{
                marginBottom: 1,
              }}
            >
              {formLabel.name}
            </FormLabel>
          )}
          <TextField
            {...field}
            value={field.value || ""}
            label={label}
            error={!!errors?.[fieldName]}
            helperText={(errors?.[fieldName]?.message as string) || ""}
            {...props}
          />
        </FormControl>
      )}
    />
  );
};

export default ControllerTextField;
