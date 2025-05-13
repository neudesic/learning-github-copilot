import {
  Box,
  FormControl,
  FormControlLabel,
  FormHelperText,
  FormLabel,
  Radio,
  RadioGroup,
} from "@mui/material";
import {
  Control,
  Controller,
  FieldErrors,
  FieldValues,
  Path,
} from "react-hook-form";
import { OptionType } from "../../types/helperTypes";

const ControllerRadioGroup = <T extends FieldValues>({
  fieldName,
  label,
  control,
  errors,
  options = [
    { id: "1", value: "yes", name: "Yes" },
    { id: "2", value: "no", name: "No" },
  ],
}: {
  fieldName: Path<T>;
  label: string;
  // eslint-disable-next-line @typescript-eslint/no-explicit-any
  control: Control<T, any>;
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
        sx={{
          marginBottom: 1,
        }}
      >
        {label}
      </FormLabel>
      <Controller
        name={fieldName}
        control={control}
        render={({ field }) => (
          <FormControl component="fieldset">
            <RadioGroup
              aria-label={label}
              {...field}
              name={fieldName}
              value={field.value || ""}
              onChange={(e) => field.onChange(e.target.value)}
              sx={{
                display: "flex",
                flexDirection: "row",
                gap: 2,
              }}
            >
              {options.map((option) => (
                <FormControlLabel
                  sx={{
                    color: errors?.[fieldName] ? "red" : "inhert",
                  }}
                  key={option.id}
                  value={option.value}
                  control={<Radio />}
                  label={option.name}
                />
              ))}
            </RadioGroup>
            {!!errors?.[fieldName] && (
              <FormHelperText error>
                {errors?.[fieldName]?.message as string}
              </FormHelperText>
            )}
          </FormControl>
        )}
      />
    </Box>
  );
};

export default ControllerRadioGroup;
