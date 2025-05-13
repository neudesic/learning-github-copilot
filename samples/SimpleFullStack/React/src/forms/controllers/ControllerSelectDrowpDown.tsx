import {
  Box,
  FormControl,
  FormHelperText,
  FormLabel,
  FormLabelProps,
  InputLabel,
  MenuItem,
  Select,
} from "@mui/material";
import { Control, Controller, FieldErrors, FieldValues } from "react-hook-form";
import { OptionType } from "../../types/helperTypes";

const ControllerSelectDropDown = <T extends FieldValues>({
  fieldName,
  label,
  control,
  errors,
  formLabel = {
    name: "Select an option",
    props: {},
  },
  options = [
    { id: "1", value: "yes", name: "Yes" },
    { id: "2", value: "no", name: "No" },
  ],
}: {
  // eslint-disable-next-line @typescript-eslint/no-explicit-any
  fieldName: any;
  label: string;
  // eslint-disable-next-line @typescript-eslint/no-explicit-any
  control: Control<T, any>;
  formLabel?: {
    name: string;
    props: FormLabelProps;
  };
  errors: FieldErrors<T>;
  options?: OptionType[];
}) => {
  return (
    <Box
      sx={{
        display: "flex",
        flexDirection: "column",
        width: "100%",
      }}
    >
      <FormLabel
        component="legend"
        {...formLabel.props}
        sx={{
          marginBottom: 1,
        }}
      >
        {formLabel.name}
      </FormLabel>
      <FormControl>
        <InputLabel
          id={`${fieldName}_label_id`}
          size="small"
          error={!!errors?.[fieldName]}
        >
          {label}
        </InputLabel>

        <Controller
          name={fieldName}
          control={control}
          render={({ field }) => (
            <Select
              size="small"
              label={"Select an option"}
              id={fieldName}
              labelId={`${fieldName}_label_id`}
              {...field}
              value={field.value || ""}
              onChange={(e) => field.onChange(e.target.value)}
              error={!!errors?.[fieldName]}
            >
              <MenuItem value="">
                <em>None</em>
              </MenuItem>
              {options.map((option) => (
                <MenuItem key={option.id} value={option.value}>
                  {option.name}
                </MenuItem>
              ))}
            </Select>
          )}
        />
        <FormHelperText error>
          {errors?.[fieldName]?.message as string}
        </FormHelperText>
      </FormControl>
    </Box>
  );
};

export default ControllerSelectDropDown;
