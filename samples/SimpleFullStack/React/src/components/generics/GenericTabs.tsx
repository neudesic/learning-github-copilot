import { Box, Tab, Tabs } from "@mui/material";
import { useState } from "react";

export type Tabs = {
  id: string;
  title: string;
  conent: React.ReactNode;
};

const GenericTabs = ({ tabs }: { tabs: Tabs[] }) => {
  const [value, setValue] = useState(0);

  const handleChange = (event: React.SyntheticEvent, newValue: number) => {
    setValue(newValue);
  };

  return (
    <Box sx={{ width: "100%" }}>
      <Box sx={{ borderBottom: 1, borderColor: "divider" }}>
        <Tabs
          value={value}
          onChange={handleChange}
          aria-label="basic tabs example"
        >
          {tabs.map((tab, index) => (
            <Tab label={tab.title} {...a11yProps(index)} />
          ))}
        </Tabs>
      </Box>
      {tabs.map((tab, index) => (
        <CustomTabPanel value={value} index={index}>
          {tab.conent}
        </CustomTabPanel>
      ))}
    </Box>
  );
};

export default GenericTabs;

function a11yProps(id: number) {
  return {
    id: `simple-tab-${id}`,
    "aria-controls": `simple-tabpanel-${id}`,
  };
}

interface TabPanelProps {
  children?: React.ReactNode;
  index: number;
  value: number;
}

function CustomTabPanel(props: TabPanelProps) {
  const { children, value, index, ...other } = props;

  return (
    <div
      role="tabpanel"
      hidden={value !== index}
      id={`simple-tabpanel-${index}`}
      aria-labelledby={`simple-tab-${index}`}
      {...other}
    >
      {value === index && <Box sx={{ p: 3 }}>{children}</Box>}
    </div>
  );
}
