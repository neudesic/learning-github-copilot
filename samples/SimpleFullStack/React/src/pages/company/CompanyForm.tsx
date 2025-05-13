import { zodResolver } from "@hookform/resolvers/zod";
import { useForm } from "react-hook-form";
import { z } from "zod";

import Box from "@mui/material/Box";

import ControllerTextField from "forms/controllers/ControllerTextField";
import ControllerSelect from "forms/controllers/ControllerSelect";
import { verticalOptions } from "constants/companyDropdownOptions";
import { Company, initialCompany } from "types/company";
import { useEffect } from "react";
import { useFormConfig } from "store/useFormConfigs";
import { useCompanies } from "store/useCompanies";
import useGlobalModal from "store/global/useGlobalModal";

const formSchema = z.object({
  tpid: z.string().min(1, { message: "Required" }),
  accountName: z.string().min(1, { message: "Required" }),
  segment: z.string().min(1, { message: "Required" }),
  industry: z.string().min(1, { message: "Required" }),
  subSegment: z.string().min(1, { message: "Required" }),
  vertical: z.string().min(1, { message: "Required" }),
});

const CompanyForm = ({
  initialData,
}: {
  initialData?: Company;
}) => {
  const { setIsDirty } = useFormConfig();
  const { addCompany, editCompany, companies } = useCompanies();
  const { closeGlobalModal } = useGlobalModal();

  const {
    handleSubmit,
    control,
    formState: { errors, isDirty },
  } = useForm<Company>({
    defaultValues: initialData || {
      ...initialCompany,
    },
    mode: "onSubmit",
    reValidateMode: "onChange",
    resolver: zodResolver(formSchema),
  });

  const handleOnSubmit = handleSubmit((data) => {
    if (initialData) {
      editCompany(initialData.id, data);
      closeGlobalModal();
      return;
    }
    const newData: Company = {
      ...initialCompany,
      ...data,
      id: companies.length.toString(),
    };

    addCompany(newData);
    closeGlobalModal();
  });

  useEffect(() => {
    setIsDirty(isDirty);
  }, [isDirty, setIsDirty]);

  return (
    <form id="company-form" onSubmit={handleOnSubmit}>
      <Box
        sx={{
          display: "flex",
          flexDirection: "column",
          gap: 3,
          paddingTop: 1,
        }}
      >
        <ControllerTextField<Company>
          fieldName={`tpid`}
          label="TPID"
          control={control}
          errors={errors}
          props={{ variant: "outlined", fullWidth: true, size: "small" }}
        />

        <ControllerTextField<Company>
          fieldName={`accountName`}
          label="Account Name"
          control={control}
          errors={errors}
          props={{ variant: "outlined", fullWidth: true, size: "small" }}
        />

        <ControllerTextField<Company>
          fieldName={`segment`}
          label="Segment"
          control={control}
          errors={errors}
          props={{ variant: "outlined", fullWidth: true, size: "small" }}
        />

        <ControllerTextField<Company>
          fieldName={`industry`}
          label="Industry"
          control={control}
          errors={errors}
          props={{ variant: "outlined", fullWidth: true, size: "small" }}
        />

        <ControllerTextField<Company>
          fieldName={`subSegment`}
          label="Sub Segment"
          control={control}
          errors={errors}
          props={{ variant: "outlined", fullWidth: true, size: "small" }}
        />

        <ControllerSelect<Company>
          fieldName="vertical"
          label="Vertical"
          options={verticalOptions}
          props={{ variant: "outlined", fullWidth: true, size: "small" }}
          control={control}
          errors={errors}
        />
      </Box>
    </form>
  );
};

export default CompanyForm;
