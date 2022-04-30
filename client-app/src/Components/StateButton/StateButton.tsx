import React, { useState } from "react";
import { GeneratePdf } from "../../Services/PdfGenerator";
import { LoadingSpinner } from "../LoadingSpinner/LoadingSpinner";

interface StateButtonProps {
  isSubmitting: boolean;
  submitForm: (() => Promise<void>) & (() => Promise<any>)
}

export const StateButton: React.FC<StateButtonProps> = (props: StateButtonProps) => {

  function Clicked() {
    props.submitForm();
  }

  return (
    <div className='justify-center text-primary text-4xl border border-primary rounded p-4 m-4 bg-dark flex items-center cursor-pointer'
      onClick={Clicked}>
      {props.isSubmitting ? "Generating" : "Generate PDF"}
      {props.isSubmitting && <LoadingSpinner />}
    </div>
  )
}