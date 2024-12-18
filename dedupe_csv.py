import csv

def dedupe_csv(input_file, output_file):
    seen = set()
    with open(input_file, 'r', newline='') as infile, open(output_file, 'w', newline='') as outfile:
        reader = csv.reader(infile)
        writer = csv.writer(outfile)
        for row in reader:
            row_tuple = tuple(row)
            if row_tuple not in seen:
                seen.add(row_tuple)
                writer.writerow(row)

if __name__ == "__main__": 
    input_file = 'input.csv'
    output_file = 'output.csv'
    dedupe_csv(input_file, output_file)
