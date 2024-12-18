import csv

def dedupe_csv(input_file, output_file):
    """
    Remove duplicate rows from a CSV file.
    
    :param input_file: Path to the input CSV file.
    :param output_file: Path to the output CSV file.
    """
    seen = set()
    with open(input_file, 'r', newline='') as infile, open(output_file, 'w', newline='') as outfile:
        reader = csv.reader(infile)
        writer = csv.writer(outfile)
        for row in reader:
            try:
                row_tuple = tuple(row)
            except Exception as e:
                print(f"Error processing row {row}: {e}")
                continue
            if row_tuple not in seen:
                seen.add(row_tuple)
                writer.writerow(row)

if __name__ == "__main__":
    input_file = 'input.csv'
    output_file = 'output.csv'
    dedupe_csv(input_file, output_file)