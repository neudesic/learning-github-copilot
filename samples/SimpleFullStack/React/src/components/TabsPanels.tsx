import { Box, Tab, Tabs } from "@mui/material";

export type Tabs = {
  id: string;
  title: string;
  conent: React.ReactNode;
};

const TabsPanels = ({
  currentTab,
  setCurrentTab,
  tabs,
}: {
  currentTab: number;
  setCurrentTab: (tab: number) => void;
  tabs: Tabs[];
}) => {
  const handleChange = (event: React.SyntheticEvent, newValue: number) => {
    setCurrentTab(newValue);
  };

  return (
    <Box sx={{ width: "100%" }}>
      <Box sx={{ borderBottom: 1, borderColor: "divider" }}>
        <Tabs
          value={currentTab}
          onChange={handleChange}
          aria-label="basic tabs example"
        >
          {tabs.map((tab, index) => (
            <Tab label={tab.title} {...a11yProps(index)} />
          ))}
        </Tabs>
      </Box>
      {tabs.map((tab, index) => (
        <CustomTabPanel value={currentTab} index={index}>
          {tab.conent}
        </CustomTabPanel>
      ))}
    </Box>
  );
};

export default TabsPanels;

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
      {/* KEEP IN DOM JUST HIDE */}
      <Box
        sx={{
          p: 3,
          display: value === index ? "block" : "none",
        }}
      >
        {children}
      </Box>
    </div>
  );
}
