import {
  Control,
  FieldValues,
  FormState,
  useForm,
  UseFormProps,
} from "react-hook-form";

const GenericFrom = <
  TFieldValues extends FieldValues = FieldValues,
  TContext = any,
>({
  formId,
  props,
  onSubmitted,
  createForm,
}: {
  formId: string;
  props: UseFormProps<TFieldValues, TContext>;
  onSubmitted?: (data: TFieldValues | undefined) => void;
  createForm: (
    control: Control<TFieldValues, any>,
    formState: FormState<TFieldValues>,
  ) => React.ReactNode;
}) => {
  const { handleSubmit, control, formState } = useForm<TFieldValues>(props);

  const onSubmit = handleSubmit((data) => {
    if (onSubmitted) {
      onSubmitted(data);
    } else {
      //default logic
    }
  });

  return (
    <form id={formId} onSubmit={onSubmit}>
      {createForm(control, formState)}
    </form>
  );
};

export default GenericFrom;
