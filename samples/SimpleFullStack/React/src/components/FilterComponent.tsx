import {
  Box,
  FormControl,
  InputLabel,
  MenuItem,
  Select,
  TextField,
} from "@mui/material";

export type Filter<TObj> = {
  selected: string | undefined;
  options: Array<TObj> | undefined;
  onChange: (selected: string) => void;
};

type FilterComponentProps<TObj extends Base> = {
  search: string | undefined;
  onSearch: (search: string) => void;
  dropdownData?: Array<Filter<TObj>>;
};

type Base = {
  id: string;
  name: string;
};

const FilterComponent = <TObj extends Base>({
  search,
  onSearch,
  dropdownData,
}: FilterComponentProps<TObj>) => {
  return (
    <Box
      sx={{
        display: "flex",
        flexDirection: "row",
        gap: 3,
      }}
    >
      <TextField
        value={search}
        onChange={(e) => onSearch(e.target.value)}
        label="Search"
        variant="outlined"
        sx={{
          flexGrow: 2,
        }}
      />

      {dropdownData &&
        dropdownData.map(({ selected, options, onChange }, index) => (
          <FormControl
            key={index}
            sx={{
              flexGrow: 1,
            }}
          >
            <InputLabel id="demo-simple-select-label">Category</InputLabel>
            <Select
              value={selected}
              labelId="demo-simple-select-label"
              id="demo-simple-select"
              label="Parent Category"
              onChange={(e) => {
                onChange(e.target.value as string);
              }}
            >
              <MenuItem value={""}>None</MenuItem>
              {options &&
                options.map((category) => (
                  <MenuItem key={category.id} value={category.id}>
                    {category.name}
                  </MenuItem>
                ))}
            </Select>
          </FormControl>
        ))}
    </Box>
  );
};

export default FilterComponent;
